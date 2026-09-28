let employees = [];
let departments = [];

document.addEventListener('DOMContentLoaded', async () => {
    auth.protectPage();
    if (auth.getRole() === 'EMPLOYEE') {
        window.location.href = 'dashboard.html';
        return;
    }
    await loadDepartments();
    loadEmployees();
});

async function loadDepartments() {
    try {
        departments = await api.get('/Department');
        const select = document.getElementById('empDept');
        departments.forEach(d => {
            const opt = document.createElement('option');
            opt.value = d.id;
            opt.textContent = d.name;
            select.appendChild(opt);
        });
    } catch (e) {
        console.error("Failed to load departments", e);
    }
}

async function loadEmployees() {
    const loading = document.getElementById('loading');
    const table = document.getElementById('empTable');
    const tbody = document.getElementById('empTableBody');
    
    try {
        employees = await api.get('/Employee');
        tbody.innerHTML = '';
        
        if (employees.length === 0) {
            tbody.innerHTML = '<tr><td colspan="6" class="text-center">No employees found.</td></tr>';
        } else {
            employees.forEach(emp => {
                const tr = document.createElement('tr');
                tr.innerHTML = `
                    <td>${emp.employeeCode}</td>
                    <td>${emp.name}</td>
                    <td>${emp.email}</td>
                    <td>${emp.designation}</td>
                    <td>$${emp.basicSalary.toFixed(2)}</td>
                    <td>
                        <button class="btn btn-sm btn-secondary" onclick="editEmp(${emp.id})">Edit</button>
                        <button class="btn btn-sm btn-danger" onclick="deleteEmp(${emp.id})">Delete</button>
                    </td>
                `;
                tbody.appendChild(tr);
            });
        }
        
        loading.classList.add('hidden');
        table.classList.remove('hidden');
    } catch (error) {
        loading.innerHTML = `<p style="color:var(--danger)">Error loading employees: ${error.message}</p>`;
    }
}

function openEmpModal() {
    document.getElementById('modalTitle').textContent = 'Add Employee';
    document.getElementById('empForm').reset();
    document.getElementById('empId').value = '';
    document.getElementById('userIdGroup').classList.remove('hidden');
    document.getElementById('empModal').classList.add('active');
}

function closeEmpModal() {
    document.getElementById('empModal').classList.remove('active');
}

function editEmp(id) {
    const emp = employees.find(e => e.id === id);
    if (!emp) return;
    
    document.getElementById('modalTitle').textContent = 'Edit Employee';
    document.getElementById('empId').value = emp.id;
    
    // UserID is not part of UpdateDto typically, but we will hide it
    document.getElementById('userIdGroup').classList.add('hidden');
    
    document.getElementById('empCode').value = emp.employeeCode;
    document.getElementById('empName').value = emp.name;
    document.getElementById('empEmail').value = emp.email;
    document.getElementById('empPhone').value = emp.phone;
    document.getElementById('empDept').value = emp.departmentId;
    document.getElementById('empDesignation').value = emp.designation;
    document.getElementById('empSalary').value = emp.basicSalary;
    
    if (emp.joiningDate) {
        document.getElementById('empJoining').value = emp.joiningDate.split('T')[0];
    }
    
    document.getElementById('empModal').classList.add('active');
}

async function saveEmployee() {
    const id = document.getElementById('empId').value;
    
    const payload = {
        departmentId: parseInt(document.getElementById('empDept').value),
        employeeCode: document.getElementById('empCode').value,
        name: document.getElementById('empName').value,
        email: document.getElementById('empEmail').value,
        phone: document.getElementById('empPhone').value,
        designation: document.getElementById('empDesignation').value,
        basicSalary: parseFloat(document.getElementById('empSalary').value),
        joiningDate: document.getElementById('empJoining').value
    };
    
    if (!id) {
        payload.userId = parseInt(document.getElementById('empUserId').value);
    }
    
    const btn = document.getElementById('saveEmpBtn');
    btn.disabled = true;
    btn.textContent = 'Saving...';
    
    try {
        if (id) {
            await api.put(`/Employee/${id}`, payload);
            showToast('Employee updated successfully');
        } else {
            await api.post('/Employee', payload);
            showToast('Employee created successfully');
        }
        closeEmpModal();
        loadEmployees();
    } catch (error) {
        showToast(error.message, 'error');
    } finally {
        btn.disabled = false;
        btn.textContent = 'Save';
    }
}

async function deleteEmp(id) {
    if (!confirm('Are you sure you want to delete this employee?')) return;
    
    try {
        await api.delete(`/Employee/${id}`);
        showToast('Employee deleted successfully');
        loadEmployees();
    } catch (error) {
        showToast(error.message, 'error');
    }
}
