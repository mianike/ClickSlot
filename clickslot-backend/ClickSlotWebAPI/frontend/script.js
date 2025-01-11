function apiFetch(endpoint, method = "GET", body = null) {
    var headers = { "Content-Type": "application/json" };
    var token = localStorage.getItem("token");
    if (token) headers["Authorization"] = `Bearer ${token}`;

    return fetch(`https://localhost:7044/${endpoint}`, {
            method,
            headers,
            body: body ? JSON.stringify(body) : null
        })
        .then(response => {
            if (!response.ok) throw new Error("API Error");
            return response.json();
        });
}