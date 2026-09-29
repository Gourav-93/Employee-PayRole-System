document.addEventListener('DOMContentLoaded', async () => {
    auth.protectPage();
    
    const user = auth.getCurrentUser();
    const loading = document.getElementById('loading');
    const content = document.getElementById('profile-content');
    
    try {
        const emp = await api.get('/employee/my-profile');
        
        document.getElementById('avatarLetter').textContent = emp.name ? emp.name.charAt(0).toUpperCase() : 'U';
        document.getElementById('profName').textContent = emp.name;
        document.getElementById('profRole').textContent = user ? user.role : 'EMPLOYEE';
        document.getElementById('profEmail').textContent = emp.email;
        document.getElementById('profPhone').textContent = emp.phone || '-';
        document.getElementById('profCode').textContent = emp.employeeCode || '-';
        document.getElementById('profDesig').textContent = emp.designation || '-';
        document.getElementById('profDept').textContent = emp.departmentId ? `Dept #${emp.departmentId}` : '-';
        document.getElementById('profJoin').textContent = emp.joiningDate ? emp.joiningDate.split('T')[0] : '-';
        
        // Set Footer based on Role
        const footer = document.getElementById('profFooter');
        if (user && (user.role === 'ADMIN' || user.role === 'HR')) {
            footer.textContent = 'You have system-level access. You can update details from the Admin panel.';
        }
        
        loading.classList.add('hidden');
        content.classList.remove('hidden');
    } catch (error) {
        // Fallback to User Info if Employee profile not found
        if (user) {
            document.getElementById('avatarLetter').textContent = user.name ? user.name.charAt(0).toUpperCase() : 'U';
            document.getElementById('profName').textContent = user.name;
            document.getElementById('profEmail').textContent = user.email;
            document.getElementById('profRole').textContent = user.role;
            
            // Try fetching full User details (which now includes Phone) to display
            try {
                const meUser = await api.get('/auth/my-profile');
                if (meUser) {
                    document.getElementById('profPhone').textContent = meUser.phone || '-';
                    // Update user in local storage to keep sync
                    localStorage.setItem('user', JSON.stringify(meUser));
                }
            } catch(e) {
                console.error("Could not fetch latest user details", e);
            }
            
            loading.classList.add('hidden');
            content.classList.remove('hidden');
            
            if (user.role === 'ADMIN' || user.role === 'HR') {
                // Hide employee specific fields
                document.querySelectorAll('.emp-data').forEach(el => el.classList.add('hidden'));
                
                // Set admin footer
                document.getElementById('profFooter').textContent = 'Administrator / HR Account. You have system-level privileges.';
                
                // Show a clean welcome toast instead of a warning
                showToast(`Welcome, ${user.name}! Admin profile loaded.`);
            } else {
                showToast('Employee profile not found. Please contact HR.', 'warning');
            }
        } else {
            loading.innerHTML = `<p style="color:var(--danger)">Failed to load profile.</p>`;
        }
    }
});

// Modal Logic
function openEditProfileModal() {
    const userStr = localStorage.getItem('user');
    if (userStr) {
        const user = JSON.parse(userStr);
        document.getElementById('editProfName').value = user.name || '';
        document.getElementById('editProfPhone').value = user.phone || document.getElementById('profPhone').textContent.replace('-', '') || '';
    }
    document.getElementById('editProfileModal').style.display = 'flex';
}

function closeEditProfileModal() {
    document.getElementById('editProfileModal').style.display = 'none';
}

async function saveProfile() {
    const name = document.getElementById('editProfName').value.trim();
    const phone = document.getElementById('editProfPhone').value.trim();
    
    if (!name) {
        showToast('Name is required', 'error');
        return;
    }
    
    const btn = document.getElementById('saveProfBtn');
    btn.textContent = 'Saving...';
    btn.disabled = true;
    
    try {
        const response = await api.put('/auth/me', {
            name: name,
            phone: phone
        });
        
        // Update UI
        document.getElementById('profName').textContent = response.name;
        document.getElementById('profPhone').textContent = response.phone || '-';
        document.getElementById('avatarLetter').textContent = response.name.charAt(0).toUpperCase();
        
        // Update localStorage
        localStorage.setItem('user', JSON.stringify(response));
        
        showToast('Profile updated successfully!');
        closeEditProfileModal();
    } catch (error) {
        showToast(error.message, 'error');
    } finally {
        btn.textContent = 'Save Changes';
        btn.disabled = false;
    }
}
