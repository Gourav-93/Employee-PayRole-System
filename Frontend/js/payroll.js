let payrolls = [];
let role = '';

document.addEventListener('DOMContentLoaded', () => {
    auth.protectPage();
    role = auth.getRole();
    
    if (role === 'EMPLOYEE') {
        document.getElementById('pageTitle').textContent = 'My Payroll';
        document.getElementById('tableTitle').textContent = 'My Salary History';
    }
    
    loadPayrolls();
});

const getMonthName = (monthNumber) => {
    const date = new Date();
    date.setMonth(monthNumber - 1);
    return date.toLocaleString('default', { month: 'short' });
};

async function loadPayrolls() {
    const loading = document.getElementById('loading');
    const table = document.getElementById('payrollTable');
    const tbody = document.getElementById('payrollTableBody');
    
    try {
        if (role === 'EMPLOYEE') {
            payrolls = await api.get('/Payroll/me');
        } else {
            payrolls = await api.get('/Payroll');
        }
        
        tbody.innerHTML = '';
        
        const dataArray = Array.isArray(payrolls) ? payrolls : [payrolls];
        
        if (dataArray.length === 0 || !dataArray[0]) {
            tbody.innerHTML = `<tr><td colspan="${role === 'EMPLOYEE' ? 6 : 8}" class="text-center">No payroll records found.</td></tr>`;
        } else {
            dataArray.forEach(pay => {
                const tr = document.createElement('tr');
                
                let actionsHtml = '';
                
                if (role !== 'EMPLOYEE') {
                    actionsHtml = `
                        <td class="admin-only">
                            <button class="btn btn-sm btn-secondary" onclick="editPayroll(${pay.id})">Edit</button>
                            <button class="btn btn-sm btn-danger" onclick="deletePayroll(${pay.id})">Delete</button>
                        </td>
                    `;
                }

                tr.innerHTML = `
                    ${role !== 'EMPLOYEE' ? `<td class="admin-only">${pay.employeeId}</td>` : ''}
                    <td>${getMonthName(pay.month)} ${pay.year}</td>
                    <td>$${pay.basicSalary.toFixed(2)}</td>
                    <td>$${pay.allowances.toFixed(2)}</td>
                    <td>$${pay.deductions.toFixed(2)}</td>
                    <td class="text-danger">-$${pay.unpaidLeaveDeduction.toFixed(2)}</td>
                    <td style="font-weight:bold; color:var(--success)">$${pay.netSalary.toFixed(2)}</td>
                    ${actionsHtml}
                `;
                tbody.appendChild(tr);
            });
        }
        
        loading.classList.add('hidden');
        table.classList.remove('hidden');
    } catch (error) {
        loading.innerHTML = `<p style="color:var(--danger)">Error loading payrolls: ${error.message}</p>`;
    }
}

function openPayrollModal() {
    document.getElementById('modalTitle').textContent = 'Generate Payroll';
    document.getElementById('payrollForm').reset();
    document.getElementById('payrollId').value = '';
    
    document.getElementById('empIdGroup').classList.remove('hidden');
    
    const d = new Date();
    document.getElementById('payMonth').value = d.getMonth() + 1;
    document.getElementById('payYear').value = d.getFullYear();
    
    document.getElementById('payrollModal').classList.add('active');
}

function closePayrollModal() {
    document.getElementById('payrollModal').classList.remove('active');
}

function editPayroll(id) {
    const dataArray = Array.isArray(payrolls) ? payrolls : [payrolls];
    const pay = dataArray.find(r => r.id === id);
    if (!pay) return;
    
    document.getElementById('modalTitle').textContent = 'Edit Payroll';
    document.getElementById('payrollId').value = pay.id;
    document.getElementById('empIdGroup').classList.add('hidden'); // Cannot update EmpId
    
    document.getElementById('payMonth').value = pay.month;
    document.getElementById('payYear').value = pay.year;
    document.getElementById('payBasic').value = pay.basicSalary;
    document.getElementById('payAllow').value = pay.allowances;
    document.getElementById('payDed').value = pay.deductions;
    document.getElementById('payUnpaidDed').value = pay.unpaidLeaveDeduction;
    
    document.getElementById('payrollModal').classList.add('active');
}

async function savePayroll() {
    const id = document.getElementById('payrollId').value;
    
    const payload = {
        month: parseInt(document.getElementById('payMonth').value),
        year: parseInt(document.getElementById('payYear').value),
        basicSalary: parseFloat(document.getElementById('payBasic').value),
        allowances: parseFloat(document.getElementById('payAllow').value),
        deductions: parseFloat(document.getElementById('payDed').value),
        unpaidLeaveDeduction: parseFloat(document.getElementById('payUnpaidDed').value)
    };
    
    if (!id) {
        payload.employeeId = parseInt(document.getElementById('payEmpId').value);
    }
    
    const btn = document.getElementById('savePayrollBtn');
    btn.disabled = true;
    btn.textContent = 'Saving...';
    
    try {
        if (id) {
            await api.put(`/Payroll/${id}`, payload);
            showToast('Payroll updated successfully');
        } else {
            await api.post('/Payroll', payload);
            showToast('Payroll generated successfully');
        }
        closePayrollModal();
        loadPayrolls();
    } catch (error) {
        showToast(error.message, 'error');
    } finally {
        btn.disabled = false;
        btn.textContent = 'Save';
    }
}

async function deletePayroll(id) {
    if (!confirm('Are you sure you want to delete this payroll record?')) return;
    try {
        await api.delete(`/Payroll/${id}`);
        showToast('Payroll record deleted');
        loadPayrolls();
    } catch (error) {
        showToast(error.message, 'error');
    }
}
