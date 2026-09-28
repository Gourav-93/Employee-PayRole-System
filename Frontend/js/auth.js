const auth = {
    login: async (email, password) => {
        try {
            const data = await api.post('/Auth/login', { email, password });
            if (data.token) {
                localStorage.setItem('jwt_token', data.token);
                localStorage.setItem('user', JSON.stringify(data.user));
                return data;
            } else {
                throw new Error("Invalid response from server.");
            }
        } catch (error) {
            throw error;
        }
    },

    register: async (name, email, password, role) => {
        try {
            return await api.post('/Auth/register', { name, email, password, role });
        } catch (error) {
            throw error;
        }
    },

    registerEmployee: async (payload) => {
        try {
            return await api.post('/Auth/register-employee', payload);
        } catch (error) {
            throw error;
        }
    },

    logout: () => {
        localStorage.removeItem('jwt_token');
        localStorage.removeItem('user');
        window.location.href = 'login.html';
    },

    getToken: () => {
        return localStorage.getItem('jwt_token');
    },

    getCurrentUser: () => {
        const userStr = localStorage.getItem('user');
        return userStr ? JSON.parse(userStr) : null;
    },

    isLoggedIn: () => {
        return !!auth.getToken();
    },

    getRole: () => {
        const user = auth.getCurrentUser();
        return user ? user.role : null;
    },

    protectPage: () => {
        if (!auth.isLoggedIn()) {
            window.location.href = 'login.html';
        }
    },

    redirectByRole: () => {
        const role = auth.getRole();
        if (role === 'ADMIN') window.location.href = 'admin-dashboard.html';
        else if (role === 'HR') window.location.href = 'hr-dashboard.html';
        else window.location.href = 'employee-dashboard.html';
    },

    updateSidebar: () => {
        const role = auth.getRole();
        const user = auth.getCurrentUser();
        
        if (user) {
            const nameEl = document.getElementById('sidebar-user-name');
            const roleEl = document.getElementById('sidebar-user-role');
            if (nameEl) nameEl.textContent = user.name;
            if (roleEl) roleEl.textContent = role;
        }

        const menu = document.querySelector('.sidebar-menu');
        if (menu) {
            let linksHtml = '';
            const currentPage = window.location.pathname.split('/').pop() || 'dashboard.html';
            const isActive = (page) => currentPage === page ? 'class="active"' : '';

            if (role === 'ADMIN') {
                linksHtml += `<li><a href="admin-dashboard.html" ${isActive('admin-dashboard.html')}>Dashboard</a></li>`;
                linksHtml += `<li><a href="employees.html" ${isActive('employees.html')}>Employees</a></li>`;
                linksHtml += `<li><a href="departments.html" ${isActive('departments.html')}>Departments</a></li>`;
                linksHtml += `<li><a href="attendance.html" ${isActive('attendance.html')}>Attendance</a></li>`;
                linksHtml += `<li><a href="leaves.html" ${isActive('leaves.html')}>Leaves</a></li>`;
                linksHtml += `<li><a href="payroll.html" ${isActive('payroll.html')}>Payroll</a></li>`;
                linksHtml += `<li><a href="profile.html" ${isActive('profile.html')}>Profile</a></li>`;
            } else if (role === 'HR') {
                linksHtml += `<li><a href="hr-dashboard.html" ${isActive('hr-dashboard.html')}>Dashboard</a></li>`;
                linksHtml += `<li><a href="employees.html" ${isActive('employees.html')}>Employees</a></li>`;
                linksHtml += `<li><a href="departments.html" ${isActive('departments.html')}>Departments</a></li>`;
                linksHtml += `<li><a href="attendance.html" ${isActive('attendance.html')}>Attendance</a></li>`;
                linksHtml += `<li><a href="leaves.html" ${isActive('leaves.html')}>Leave Management</a></li>`;
                linksHtml += `<li><a href="payroll.html" ${isActive('payroll.html')}>Payroll</a></li>`;
                linksHtml += `<li><a href="profile.html" ${isActive('profile.html')}>Profile</a></li>`;
            } else {
                linksHtml += `<li><a href="employee-dashboard.html" ${isActive('employee-dashboard.html')}>Dashboard</a></li>`;
                linksHtml += `<li><a href="profile.html" ${isActive('profile.html')}>My Profile</a></li>`;
                linksHtml += `<li><a href="attendance.html" ${isActive('attendance.html')}>My Attendance</a></li>`;
                linksHtml += `<li><a href="leaves.html" ${isActive('leaves.html')}>My Leaves</a></li>`;
                linksHtml += `<li><a href="payroll.html" ${isActive('payroll.html')}>My Payroll</a></li>`;
            }
            linksHtml += `<li><a href="#" id="dynamic-logout-btn">Logout</a></li>`;
            
            menu.innerHTML = linksHtml;

            const logoutBtn = document.getElementById('dynamic-logout-btn');
            if (logoutBtn) {
                logoutBtn.addEventListener('click', (e) => {
                    e.preventDefault();
                    auth.logout();
                });
            }
        }
    }
};

document.addEventListener('DOMContentLoaded', () => {
    const logoutBtn = document.getElementById('logout-btn');
    if (logoutBtn) {
        logoutBtn.addEventListener('click', (e) => {
            e.preventDefault();
            auth.logout();
        });
    }

    if (document.querySelector('.sidebar')) {
        auth.updateSidebar();
    }

    // Hide admin-only elements for employees
    if (auth.getRole() === 'EMPLOYEE') {
        document.querySelectorAll('.admin-only').forEach(el => el.style.display = 'none');
    }
});
