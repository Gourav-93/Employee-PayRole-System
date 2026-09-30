const API_BASE_URL = 'http://localhost:5236/api'; 

async function apiRequest(endpoint, method = 'GET', body = null) {
    const token = localStorage.getItem('jwt_token');
    const headers = {
        'Content-Type': 'application/json'
    };

    if (token) {
        headers['Authorization'] = `Bearer ${token}`;
    }

    const options = {
        method,
        headers
    };

    if (body) {
        options.body = JSON.stringify(body);
    }

    try {
        const response = await fetch(`${API_BASE_URL}${endpoint}`, options);
        
        // Handle no content
        if (response.status === 204) {
            return null;
        }

        const data = await response.text();
        let parsedData = null;
        if (data) {
            try {
                parsedData = JSON.parse(data);
            } catch(e) {
                parsedData = { message: data };
            }
        }

        if (!response.ok) {
            if (response.status === 401) {
                // Auto logout on 401
                localStorage.removeItem('jwt_token');
                localStorage.removeItem('user');
                window.location.href = 'login.html';
                throw new Error("Session expired. Please login again.");
            }
            if (response.status === 403) {
                throw new Error("You are not authorized to perform this action.");
            }
            
            const errorMsg = parsedData?.error || parsedData?.message || parsedData || "Something went wrong on the server.";
            throw new Error(typeof errorMsg === 'string' ? errorMsg : JSON.stringify(errorMsg));
        }

        return parsedData;
    } catch (error) {
        throw error;
    }
}

const api = {
    get: (endpoint) => apiRequest(endpoint, 'GET'),
    post: (endpoint, body) => apiRequest(endpoint, 'POST', body),
    put: (endpoint, body) => apiRequest(endpoint, 'PUT', body),
    delete: (endpoint) => apiRequest(endpoint, 'DELETE')
};

function showToast(message, type = 'success') {
    let container = document.getElementById('toast-container');
    if (!container) {
        container = document.createElement('div');
        container.id = 'toast-container';
        document.body.appendChild(container);
    }

    const toast = document.createElement('div');
    toast.className = `toast ${type}`;
    toast.textContent = message;

    container.appendChild(toast);

    setTimeout(() => {
        toast.style.opacity = '0';
        setTimeout(() => toast.remove(), 300);
    }, 3000);
}
