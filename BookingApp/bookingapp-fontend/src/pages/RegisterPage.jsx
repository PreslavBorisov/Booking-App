import { useState } from "react";
import { useNavigate } from "react-router-dom";
import api from "../api/axios";
import { useAuth } from "../context/AuthContext";

function RegisterPage() {
  const navigate = useNavigate();
  const { login } = useAuth();

  const [form, setForm] = useState({
    email: "",
    password: "",
  });

  const [error, setError] = useState("");

  const onChange = (e) => {
    setForm((prev) => ({
      ...prev,
      [e.target.name]: e.target.value,
    }));
  };

  const onSubmit = async (e) => {
    e.preventDefault();
    setError("");

    try {
      const response = await api.post("/auth/register", form);
      login(response.data);
      navigate("/");
    } catch (err) {
      setError(err.response?.data?.message || "Registration failed.");
    }
  };

  return (
    <div className="mx-auto max-w-md rounded bg-white p-6 shadow">
      <h1 className="mb-6 text-2xl font-bold">Register</h1>

      <form onSubmit={onSubmit} className="space-y-4">
        <input
          name="email"
          type="email"
          placeholder="Email"
          value={form.email}
          onChange={onChange}
          className="w-full rounded border px-4 py-2"
        />
        <input
          name="password"
          type="password"
          placeholder="Password"
          value={form.password}
          onChange={onChange}
          className="w-full rounded border px-4 py-2"
        />

        {error && <p className="text-sm text-red-600">{error}</p>}

        <button className="w-full rounded bg-blue-600 px-4 py-2 text-white hover:bg-blue-700">
          Register
        </button>
      </form>
    </div>
  );
}

export default RegisterPage;