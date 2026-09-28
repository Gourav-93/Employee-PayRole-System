let departments = [];

document.addEventListener('DOMContentLoaded', () => {
    auth.protectPage();
    if (auth.getRole() === 'EMPLOYEE') {
        window.location.href = 'dashboard.html';
        return;
    }
    loadDepartments();
});

async function loadDepartments() {
    const loading = document.getElementById('loading');
    const table = document.getElementById('deptTable');
    const tbody = document.getElementById('deptTableBody');
    
    try {
        departments = await api.get('/Department');
        tbody.innerHTML = '';
        
        if (departments.length === 0) {
            tbody.innerHTML = '<tr><td colspan="4" class="text-center">No departments found.</td></tr>';
        } else {
            departments.forEach(dept => {
                const tr = document.createElement('tr');
                tr.innerHTML = `
                    <td>${dept.id}</td>
                    <td>${dept.name}</td>
                    <td>${dept.description || '-'}</td>
                    <td>
                        <button class="btn btn-sm btn-secondary" onclick="editDept(${dept.id})">Edit</button>
                        <button class="btn btn-sm btn-danger" onclick="deleteDept(${dept.id})">Delete</button>
                    </td>
                `;
                tbody.appendChild(tr);
            });
        }
        
        loading.classList.add('hidden');
        table.classList.remove('hidden');
    } catch (error) {
        loading.innerHTML = `<p style="color:var(--danger)">Error loading departments: ${error.message}</p>`;
    }
}

function openDeptModal() {
    document.getElementById('modalTitle').textContent = 'Add Department';
    document.getElementById('deptForm').reset();
    document.getElementById('deptId').value = '';
    document.getElementById('deptModal').classList.add('active');
}

function closeDeptModal() {
    document.getElementById('deptModal').classList.remove('active');
}

function editDept(id) {
    const dept = departments.find(d => d.id === id);
    if (!dept) return;
    
    document.getElementById('modalTitle').textContent = 'Edit Department';
    document.getElementById('deptId').value = dept.id;
    document.getElementById('deptName').value = dept.name;
    document.getElementById('deptDesc').value = dept.description;
    document.getElementById('deptModal').classList.add('active');
}

async function saveDepartment() {
    const id = document.getElementById('deptId').value;
    const name = document.getElementById('deptName').value;
    const desc = document.getElementById('deptDesc').value;
    
    if (!name) {
        showToast('Name is required', 'error');
        return;
    }
    
    const payload = {
        name: name,
        description: desc
    };
    
    const btn = document.getElementById('saveDeptBtn');
    btn.disabled = true;
    btn.textContent = 'Saving...';
    
    try {
        if (id) {
            await api.put(`/Department/${id}`, payload);
            showToast('Department updated successfully');
        } else {
            await api.post('/Department', payload);
            showToast('Department created successfully');
        }
        closeDeptModal();
        loadDepartments();
    } catch (error) {
        showToast(error.message, 'error');
    } finally {
        btn.disabled = false;
        btn.textContent = 'Save';
    }
}

async function deleteDept(id) {
    if (!confirm('Are you sure you want to delete this department?')) return;
    
    try {
        await api.delete(`/Department/${id}`);
        showToast('Department deleted successfully');
        loadDepartments();
    } catch (error) {
        showToast(error.message, 'error');
    }
}
