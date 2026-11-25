import axios from "axios";

const origins = {
  auth: import.meta.env.VITE_IDENTITY_URL ?? "http://localhost:5023",
  room: import.meta.env.VITE_ROOMS_URL ?? "http://localhost:5022",
  booking: import.meta.env.VITE_BOOKINGS_URL ?? "http://localhost:5021",
  admin: import.meta.env.VITE_BOOKINGS_URL ?? "http://localhost:5021",
};

const api = axios.create();

api.interceptors.request.use((config) => {
  const resource = /^\/([^/?]+)/.exec(config.url)?.[1];
  config.baseURL = origins[resource] + "/api";
  const token = localStorage.getItem("token");

  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
  }

  return config;
});

export default api;
