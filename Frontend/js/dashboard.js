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
        try {
            // Get user info and basic profile
            const userStr = localStorage.getItem('user');
            if (userStr) {
                const user = JSON.parse(userStr);
                document.getElementById('emp-initials').textContent = user.name ? user.name.charAt(0).toUpperCase() : 'U';
                document.getElementById('emp-profile-name').textContent = user.name;
            }

            // Fetch full profile for code, designation, department
            try {
                const profile = await api.get('/employee/my-profile');
                if (profile) {
                    document.getElementById('emp-profile-designation').textContent = profile.designation || 'Employee';
                    document.getElementById('emp-profile-code').textContent = profile.employeeCode || 'N/A';
                    // We might not have department name populated deeply, fallback to ID if null
                    document.getElementById('emp-profile-department').textContent = profile.department ? profile.department.name : 'Dept ' + profile.departmentId;
                }
            } catch (e) {
                console.error("Failed to fetch full profile", e);
            }

            // Fetch today's attendance to set button state
            await updateDashboardAttendanceState();

        } catch (e) {
            console.error("Error loading employee dashboard", e);
        }

        loading.classList.add('hidden');
        empContent.classList.remove('hidden');
    }
});

let todayAttRecord = null;

async function updateDashboardAttendanceState() {
    try {
        const todayAtt = await api.get('/Attendance/today');
        const btn = document.getElementById('btn-quick-checkin');
        const statusText = document.getElementById('emp-today-time');
        
        todayAttRecord = todayAtt;

        if (!todayAtt) {
            btn.textContent = "Check In Now";
            btn.className = "btn btn-primary";
            btn.onclick = () => quickMarkAttendance('check-in');
            statusText.textContent = "You haven't checked in today.";
        } else if (!todayAtt.checkOut) {
            btn.textContent = "Check Out Now";
            btn.className = "btn btn-warning";
            btn.onclick = () => quickMarkAttendance('check-out');
            statusText.textContent = `Checked In: ${todayAtt.checkIn}`;
        } else {
            btn.textContent = "Completed";
            btn.className = "btn btn-success";
            btn.disabled = true;
            statusText.textContent = `In: ${todayAtt.checkIn} | Out: ${todayAtt.checkOut}`;
        }
    } catch (e) {
        console.error("Failed to load attendance", e);
    }
}

async function quickMarkAttendance(type) {
    const btn = document.getElementById('btn-quick-checkin');
    btn.disabled = true;
    btn.textContent = "Wait...";
    
    try {
        await api.post(`/Attendance/${type}`);
        showToast(`Successfully ${type.replace('-', ' ')}ed!`);
        await updateDashboardAttendanceState();
    } catch (error) {
        showToast(error.message, 'error');
        btn.disabled = false;
        btn.textContent = type === 'check-in' ? "Check In Now" : "Check Out Now";
    }
}

function handleCheckIn() {
    // Handled directly by button onclick, but card is also clickable
    // We only trigger if the button is not disabled
    const btn = document.getElementById('btn-quick-checkin');
    if(!btn.disabled) {
        btn.click();
    }
}
