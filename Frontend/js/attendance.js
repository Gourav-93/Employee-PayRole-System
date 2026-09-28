let records = [];
let employeesList = [];

document.addEventListener('DOMContentLoaded', async () => {
    auth.protectPage();
    const role = auth.getRole();
    
    if (role === 'EMPLOYEE') {
        document.getElementById('pageTitle').textContent = 'My Attendance';
        document.getElementById('tableTitle').classList.add('hidden');
        document.getElementById('historyTitle').classList.remove('hidden');
        document.getElementById('employeeAttendanceSection').classList.remove('hidden');
        await loadTodayAttendance();
    } else {
        await loadEmployees();
    }
    
    loadAttendance();
});

async function loadTodayAttendance() {
    const statusDiv = document.getElementById('todayAttStatus');
    const btnIn = document.getElementById('btnCheckIn');
    const btnOut = document.getElementById('btnCheckOut');
    
    btnIn.classList.add('hidden');
    btnOut.classList.add('hidden');
    
    try {
        const today = await api.get('/Attendance/today');
        
        if (!today) {
            statusDiv.innerHTML = '<strong>Status:</strong> Not Marked';
            btnIn.classList.remove('hidden');
        } else {
            let html = `<strong>Status:</strong> <span class="badge ${today.status === 'Present' ? 'badge-success' : 'badge-info'}">${today.status}</span><br>`;
            html += `<strong style="margin-top:0.5rem;display:inline-block;">Check In:</strong> ${today.checkIn || '-'}<br>`;
            
            if (today.checkOut) {
                html += `<strong>Check Out:</strong> ${today.checkOut}`;
            } else {
                btnOut.classList.remove('hidden');
            }
            
            statusDiv.innerHTML = html;
        }
    } catch (e) {
        statusDiv.innerHTML = `<span style="color:var(--danger)">Error loading today's attendance.</span>`;
    }
}

async function markAttendance(type) {
    const btnIn = document.getElementById('btnCheckIn');
    const btnOut = document.getElementById('btnCheckOut');
    
    if (type === 'check-in') {
        btnIn.disabled = true;
        btnIn.textContent = 'Wait...';
    } else {
        btnOut.disabled = true;
        btnOut.textContent = 'Wait...';
    }
    
    try {
        await api.post(`/Attendance/${type}`);
        showToast(`Successfully ${type.replace('-', ' ')}ed!`);
        await loadTodayAttendance();
        await loadAttendance(); // Refresh history
    } catch (error) {
        showToast(error.message, 'error');
    } finally {
        if (type === 'check-in') {
            btnIn.disabled = false;
            btnIn.textContent = 'CHECK IN';
        } else {
            btnOut.disabled = false;
            btnOut.textContent = 'CHECK OUT';
        }
    }
}

async function loadEmployees() {
    try {
        employeesList = await api.get('/Employee');
        const select = document.getElementById('attEmpId');
        employeesList.forEach(e => {
            const opt = document.createElement('option');
            opt.value = e.id;
            opt.textContent = `${e.employeeCode} - ${e.name}`;
            select.appendChild(opt);
        });
    } catch (e) {
        console.error("Failed to load employees for attendance", e);
    }
}

async function loadAttendance() {
    const loading = document.getElementById('loading');
    const table = document.getElementById('attTable');
    const tbody = document.getElementById('attTableBody');
    const role = auth.getRole();
    
    try {
        if (role === 'EMPLOYEE') {
            records = await api.get('/Attendance/me');
        } else {
            records = await api.get('/Attendance');
        }
        
        tbody.innerHTML = '';
        
        // Handle if records is an object instead of array (sometimes /me returns one record or a list)
        // Usually /me returns a list of records for that employee. Assuming array.
        const dataArray = Array.isArray(records) ? records : [records];
        
        if (dataArray.length === 0 || !dataArray[0]) {
            tbody.innerHTML = `<tr><td colspan="${role === 'EMPLOYEE' ? 4 : 6}" class="text-center">No attendance records found.</td></tr>`;
        } else {
            dataArray.forEach(rec => {
                const tr = document.createElement('tr');
                
                let badgeClass = 'badge-info';
                if (rec.status === 'Present') badgeClass = 'badge-success';
                else if (rec.status === 'Absent') badgeClass = 'badge-danger';
                else if (rec.status === 'HalfDay') badgeClass = 'badge-warning';
                
                let actionsHtml = '';
                if (role !== 'EMPLOYEE') {
                    actionsHtml = `
                        <td class="admin-only">
                            <button class="btn btn-sm btn-secondary" onclick="editAtt(${rec.id})">Edit</button>
                            <button class="btn btn-sm btn-danger" onclick="deleteAtt(${rec.id})">Delete</button>
                        </td>
                    `;
                }

                let empDisplay = '';
                if (role !== 'EMPLOYEE') {
                    if (rec.employee) {
                        empDisplay = `${rec.employee.employeeCode} - ${rec.employee.name}`;
                    } else {
                        empDisplay = rec.employeeId;
                    }
                }

                tr.innerHTML = `
                    ${role !== 'EMPLOYEE' ? `<td class="admin-only">${empDisplay}</td>` : ''}
                    <td>${rec.date ? rec.date.split('T')[0] : '-'}</td>
                    <td>${rec.checkIn || '-'}</td>
                    <td>${rec.checkOut || '-'}</td>
                    <td>${rec.workingHours ? rec.workingHours : '-'}</td>
                    <td><span class="badge ${badgeClass}">${rec.status}</span></td>
                    <td>${rec.remarks || '-'}</td>
                    ${actionsHtml}
                `;
                tbody.appendChild(tr);
            });
        }
        
        loading.classList.add('hidden');
        table.classList.remove('hidden');
    } catch (error) {
        loading.innerHTML = `<p style="color:var(--danger)">Error loading attendance: ${error.message}</p>`;
    }
}

function openAttModal() {
    document.getElementById('modalTitle').textContent = 'Add Attendance';
    document.getElementById('attForm').reset();
    document.getElementById('attId').value = '';
    document.getElementById('empIdGroup').classList.remove('hidden');
    document.getElementById('attRemarks').value = '';
    document.getElementById('attModal').classList.add('active');
}

function closeAttModal() {
    document.getElementById('attModal').classList.remove('active');
}

function editAtt(id) {
    const dataArray = Array.isArray(records) ? records : [records];
    const rec = dataArray.find(r => r.id === id);
    if (!rec) return;
    
    document.getElementById('modalTitle').textContent = 'Edit Attendance';
    document.getElementById('attId').value = rec.id;
    
    document.getElementById('empIdGroup').classList.add('hidden'); // Cannot update EmpId easily
    
    if (rec.date) {
        document.getElementById('attDate').value = rec.date.split('T')[0];
    }
    
    document.getElementById('attCheckIn').value = rec.checkIn || '';
    document.getElementById('attCheckOut').value = rec.checkOut || '';
    document.getElementById('attStatus').value = rec.status;
    document.getElementById('attRemarks').value = rec.remarks || '';
    
    document.getElementById('attModal').classList.add('active');
}

async function saveAttendance() {
    const id = document.getElementById('attId').value;
    
    const payload = {
        date: document.getElementById('attDate').value,
        checkIn: document.getElementById('attCheckIn').value || null,
        checkOut: document.getElementById('attCheckOut').value || null,
        status: document.getElementById('attStatus').value,
        remarks: document.getElementById('attRemarks').value
    };
    
    // Add timespan format support if needed. The API expects TimeSpan? (e.g. "09:00:00")
    if (payload.checkIn && payload.checkIn.length === 5) payload.checkIn += ':00';
    if (payload.checkOut && payload.checkOut.length === 5) payload.checkOut += ':00';
    
    if (!id) {
        payload.employeeId = parseInt(document.getElementById('attEmpId').value);
    }
    
    const btn = document.getElementById('saveAttBtn');
    btn.disabled = true;
    btn.textContent = 'Saving...';
    
    try {
        if (id) {
            await api.put(`/Attendance/${id}`, payload);
            showToast('Attendance updated successfully');
        } else {
            await api.post('/Attendance', payload);
            showToast('Attendance created successfully');
        }
        closeAttModal();
        loadAttendance();
    } catch (error) {
        showToast(error.message, 'error');
    } finally {
        btn.disabled = false;
        btn.textContent = 'Save';
    }
}

async function deleteAtt(id) {
    if (!confirm('Are you sure you want to delete this record?')) return;
    
    try {
        await api.delete(`/Attendance/${id}`);
        showToast('Attendance deleted successfully');
        loadAttendance();
    } catch (error) {
        showToast(error.message, 'error');
    }
}
