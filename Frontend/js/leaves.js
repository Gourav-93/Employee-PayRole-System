let leaves = [];
let role = '';

document.addEventListener('DOMContentLoaded', () => {
    auth.protectPage();
    role = auth.getRole();
    
    if (role === 'EMPLOYEE') {
        document.getElementById('pageTitle').textContent = 'My Leaves';
        document.getElementById('tableTitle').textContent = 'My Leave Requests';
        document.getElementById('applyLeaveBtn').style.display = 'inline-block';
    } else {
        document.getElementById('applyLeaveBtn').style.display = 'none';
        // Admin shouldn't apply leave via this form, only update/approve
    }
    
    loadLeaves();
});

async function loadLeaves() {
    const loading = document.getElementById('loading');
    const table = document.getElementById('leaveTable');
    const tbody = document.getElementById('leaveTableBody');
    
    try {
        if (role === 'EMPLOYEE') {
            leaves = await api.get('/LeaveRequest/me');
        } else {
            leaves = await api.get('/LeaveRequest');
        }
        
        tbody.innerHTML = '';
        
        const dataArray = Array.isArray(leaves) ? leaves : [leaves];
        
        if (dataArray.length === 0 || !dataArray[0]) {
            tbody.innerHTML = `<tr><td colspan="${role === 'EMPLOYEE' ? 6 : 7}" class="text-center">No leave requests found.</td></tr>`;
        } else {
            dataArray.forEach(leave => {
                const tr = document.createElement('tr');
                
                // Note: The C# enum is likely 0=Pending, 1=Approved, 2=Rejected, or serialized as string
                let statusStr = leave.status;
                if(typeof leave.status === 'number') {
                    if(leave.status === 0) statusStr = 'Pending';
                    if(leave.status === 1) statusStr = 'Approved';
                    if(leave.status === 2) statusStr = 'Rejected';
                }

                let badgeClass = 'badge-warning';
                if (statusStr === 'Approved') badgeClass = 'badge-success';
                else if (statusStr === 'Rejected') badgeClass = 'badge-danger';
                
                let actionsHtml = '';
                
                if (role !== 'EMPLOYEE') {
                    if (statusStr === 'Pending' || statusStr === 0) {
                        actionsHtml = `
                            <button class="btn btn-sm btn-success" onclick="approveLeave(${leave.id})">Approve</button>
                            <button class="btn btn-sm btn-danger" onclick="rejectLeave(${leave.id})">Reject</button>
                            <button class="btn btn-sm btn-secondary" onclick="editLeave(${leave.id})">Edit</button>
                        `;
                    } else {
                        actionsHtml = `
                            <button class="btn btn-sm btn-secondary" onclick="editLeave(${leave.id})">Edit</button>
                            <button class="btn btn-sm btn-danger" onclick="deleteLeave(${leave.id})">Delete</button>
                        `;
                    }
                } else {
                    // Employee actions
                    if (statusStr === 'Pending' || statusStr === 0) {
                        actionsHtml = `
                            <span class="text-muted">Wait for approval</span>
                        `;
                    } else {
                        actionsHtml = `
                            <span class="text-muted">Processed</span>
                        `;
                    }
                }

                tr.innerHTML = `
                    ${role !== 'EMPLOYEE' ? `<td class="admin-only">${leave.employeeId}</td>` : ''}
                    <td>${leave.leaveType}</td>
                    <td>${leave.fromDate ? leave.fromDate.split('T')[0] : '-'}</td>
                    <td>${leave.toDate ? leave.toDate.split('T')[0] : '-'}</td>
                    <td>${leave.reason}</td>
                    <td><span class="badge ${badgeClass}">${statusStr}</span></td>
                    <td>${actionsHtml}</td>
                `;
                tbody.appendChild(tr);
            });
        }
        
        loading.classList.add('hidden');
        table.classList.remove('hidden');
    } catch (error) {
        loading.innerHTML = `<p style="color:var(--danger)">Error loading leaves: ${error.message}</p>`;
    }
}

function openLeaveModal() {
    document.getElementById('modalTitle').textContent = 'Apply Leave';
    document.getElementById('leaveForm').reset();
    document.getElementById('leaveId').value = '';
    
    // Admin shouldn't apply leave, and employee ID is auto-detected by backend
    document.getElementById('empIdGroup').style.display = 'none';
    
    document.getElementById('leaveModal').classList.add('active');
}

function closeLeaveModal() {
    document.getElementById('leaveModal').classList.remove('active');
}

function editLeave(id) {
    const dataArray = Array.isArray(leaves) ? leaves : [leaves];
    const leave = dataArray.find(r => r.id === id);
    if (!leave) return;
    
    document.getElementById('modalTitle').textContent = 'Edit Leave';
    document.getElementById('leaveId').value = leave.id;
    document.getElementById('empIdGroup').style.display = 'none';
    
    document.getElementById('leaveType').value = leave.leaveType;
    if (leave.fromDate) document.getElementById('leaveFrom').value = leave.fromDate.split('T')[0];
    if (leave.toDate) document.getElementById('leaveTo').value = leave.toDate.split('T')[0];
    document.getElementById('leaveReason').value = leave.reason;
    
    document.getElementById('leaveModal').classList.add('active');
}

async function saveLeave() {
    const id = document.getElementById('leaveId').value;
    
    const fromDate = document.getElementById('leaveFrom').value;
    const toDate = document.getElementById('leaveTo').value;
    
    if (new Date(toDate) < new Date(fromDate)) {
        showToast('To Date cannot be before From Date', 'error');
        return;
    }
    
    const payload = {
        leaveType: document.getElementById('leaveType').value,
        fromDate: fromDate,
        toDate: toDate,
        reason: document.getElementById('leaveReason').value
    };
    
    // EmployeeId is now automatically determined by the backend from the JWT token
    
    const btn = document.getElementById('saveLeaveBtn');
    btn.disabled = true;
    btn.textContent = 'Saving...';
    
    try {
        if (id) {
            await api.put(`/LeaveRequest/${id}`, payload);
            showToast('Leave request updated successfully');
        } else {
            await api.post('/LeaveRequest', payload);
            showToast('Leave requested successfully');
        }
        closeLeaveModal();
        loadLeaves();
    } catch (error) {
        showToast(error.message, 'error');
    } finally {
        btn.disabled = false;
        btn.textContent = 'Submit';
    }
}

async function approveLeave(id) {
    if (!confirm('Approve this leave request?')) return;
    try {
        await api.put(`/LeaveRequest/${id}/approve`, {});
        showToast('Leave approved');
        loadLeaves();
    } catch (error) {
        showToast(error.message, 'error');
    }
}

async function rejectLeave(id) {
    if (!confirm('Reject this leave request?')) return;
    try {
        await api.put(`/LeaveRequest/${id}/reject`, {});
        showToast('Leave rejected');
        loadLeaves();
    } catch (error) {
        showToast(error.message, 'error');
    }
}

async function deleteLeave(id) {
    if (!confirm('Are you sure you want to delete this record?')) return;
    try {
        await api.delete(`/LeaveRequest/${id}`);
        showToast('Leave record deleted');
        loadLeaves();
    } catch (error) {
        showToast(error.message, 'error');
    }
}
