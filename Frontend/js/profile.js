document.addEventListener('DOMContentLoaded', async () => {
    auth.protectPage();
    
    const user = auth.getCurrentUser();
    const loading = document.getElementById('loading');
    const content = document.getElementById('profile-content');
    
    try {
        const emp = await api.get('/Employee/me');
        
        document.getElementById('avatarLetter').textContent = emp.name ? emp.name.charAt(0).toUpperCase() : 'U';
        document.getElementById('profName').textContent = emp.name;
        document.getElementById('profRole').textContent = user ? user.role : 'EMPLOYEE';
        document.getElementById('profEmail').textContent = emp.email;
        document.getElementById('profPhone').textContent = emp.phone || '-';
        document.getElementById('profCode').textContent = emp.employeeCode || '-';
        document.getElementById('profDesig').textContent = emp.designation || '-';
        document.getElementById('profDept').textContent = emp.departmentId ? `Dept #${emp.departmentId}` : '-';
        document.getElementById('profJoin').textContent = emp.joiningDate ? emp.joiningDate.split('T')[0] : '-';
        
        loading.classList.add('hidden');
        content.classList.remove('hidden');
    } catch (error) {
        // Fallback to User Info if Employee profile not found
        if (user) {
            document.getElementById('avatarLetter').textContent = user.name ? user.name.charAt(0).toUpperCase() : 'U';
            document.getElementById('profName').textContent = user.name;
            document.getElementById('profEmail').textContent = user.email;
            document.getElementById('profRole').textContent = user.role;
            
            loading.classList.add('hidden');
            content.classList.remove('hidden');
            
            if (user.role === 'ADMIN' || user.role === 'HR') {
                showToast('You are an Admin/HR without a linked Employee profile.', 'info');
            } else {
                showToast('Employee profile not found. Please contact HR.', 'warning');
            }
        } else {
            loading.innerHTML = `<p style="color:var(--danger)">Failed to load profile.</p>`;
        }
    }
});
