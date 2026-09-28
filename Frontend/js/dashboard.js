document.addEventListener('DOMContentLoaded', async () => {
    auth.protectPage();
    
    const role = auth.getRole();
    const loading = document.getElementById('loading');
    const adminContent = document.getElementById('dashboard-content');
    const empContent = document.getElementById('employee-dashboard-content');

    if (role === 'ADMIN' || role === 'HR') {
        try {
            const data = await api.get('/Dashboard');
            
            document.getElementById('stat-emp').textContent = data.totalEmployees;
            document.getElementById('stat-dept').textContent = data.totalDepartments;
            document.getElementById('stat-present').textContent = data.presentToday;
            document.getElementById('stat-absent').textContent = data.absentToday;
            document.getElementById('stat-leaves-pending').textContent = data.pendingLeaves;
            document.getElementById('stat-leaves-approved').textContent = data.approvedLeaves;
            document.getElementById('stat-payroll').textContent = data.totalPayrollRecords;
            
            loading.classList.add('hidden');
            adminContent.classList.remove('hidden');
        } catch (error) {
            loading.innerHTML = `<p style="color:var(--danger)">Failed to load dashboard: ${error.message}</p>`;
        }
    } else {
        // Employee dashboard view
        loading.classList.add('hidden');
        empContent.classList.remove('hidden');
    }
});
