import { useEffect, useState } from "react";
import api from "../api/axios";
import BookingCard from "../components/BookingCard";

function AdminDashboardPage() {
  const [stats, setStats] = useState(null);
  const [bookings, setBookings] = useState([]);
  const [message, setMessage] = useState("");
  const [error, setError] = useState("");

  const [roomForm, setRoomForm] = useState({
    name: "",
    description: "",
    pricePerNight: "",
    capacity: "",
    imageUrl: "",
    location: "",
    address: "",
    amenities: "",
  });

  const loadStats = async () => {
    try {
      const response = await api.get("/admin/stats");
      setStats(response.data);
    } catch (error) {
      console.error("Failed to load admin stats", error);
    }
  };

  const loadBookings = async () => {
    try {
      const response = await api.get("/booking");
      setBookings(response.data);
    } catch (error) {
      console.error("Failed to load bookings", error);
    }
  };

  useEffect(() => {
    loadStats();
    loadBookings();
  }, []);

  const handleRoomChange = (e) => {
    setRoomForm((prev) => ({
      ...prev,
      [e.target.name]: e.target.value,
    }));
  };

  const handleCreateRoom = async (e) => {
    e.preventDefault();
    setMessage("");
    setError("");

    try {
      const payload = {
        name: roomForm.name,
        description: roomForm.description,
        pricePerNight: Number(roomForm.pricePerNight),
        capacity: Number(roomForm.capacity),
        imageUrl: roomForm.imageUrl,
        location: roomForm.location,
        address: roomForm.address,
        amenities: roomForm.amenities
          .split(",")
          .map((a) => a.trim())
          .filter((a) => a.length > 0),
      };

      await api.post("/room", payload);

      setMessage("Room created successfully.");

      setRoomForm({
        name: "",
        description: "",
        pricePerNight: "",
        capacity: "",
        imageUrl: "",
        location: "",
        address: "",
        amenities: "",
      });

      await loadStats();
    } catch (err) {
      setError(err.response?.data?.message || "Failed to create room.");
    }
  };

  const confirmBooking = async (id) => {
    setMessage("");
    setError("");

    try {
      await api.patch(`/booking/${id}/confirm`);
      setMessage("Booking confirmed successfully.");
      await loadBookings();
      await loadStats();
    } catch (err) {
      setError(err.response?.data?.message || "Failed to confirm booking.");
    }
  };

  const cancelBooking = async (id) => {
    setMessage("");
    setError("");

    try {
      await api.delete(`/booking/${id}`);
      setMessage("Booking cancelled successfully.");
      await loadBookings();
      await loadStats();
    } catch (err) {
      setError(err.response?.data?.message || "Failed to cancel booking.");
    }
  };

  if (!stats) {
    return <p>Loading admin dashboard...</p>;
  }

  return (
    <div className="space-y-8">
      <div>
        <h1 className="mb-6 text-3xl font-bold">Admin Dashboard</h1>

        {message && (
          <div className="mb-4 rounded bg-green-100 p-3 text-green-700">
            {message}
          </div>
        )}

        {error && (
          <div className="mb-4 rounded bg-red-100 p-3 text-red-700">
            {error}
          </div>
        )}

        <div className="grid gap-4 md:grid-cols-2 lg:grid-cols-3">
          <div className="rounded bg-white p-4 shadow">
            <p className="text-sm text-gray-500">Total Rooms</p>
            <p className="text-2xl font-bold">{stats.totalRooms}</p>
          </div>

          <div className="rounded bg-white p-4 shadow">
            <p className="text-sm text-gray-500">Active Rooms</p>
            <p className="text-2xl font-bold">{stats.activeRooms}</p>
          </div>

          <div className="rounded bg-white p-4 shadow">
            <p className="text-sm text-gray-500">Total Users</p>
            <p className="text-2xl font-bold">{stats.totalUsers}</p>
          </div>

          <div className="rounded bg-white p-4 shadow">
            <p className="text-sm text-gray-500">Total Bookings</p>
            <p className="text-2xl font-bold">{stats.totalBookings}</p>
          </div>

          <div className="rounded bg-white p-4 shadow">
            <p className="text-sm text-gray-500">Pending Bookings</p>
            <p className="text-2xl font-bold">{stats.pendingBookings}</p>
          </div>

          <div className="rounded bg-white p-4 shadow">
            <p className="text-sm text-gray-500">Confirmed Bookings</p>
            <p className="text-2xl font-bold">{stats.confirmedBookings}</p>
          </div>

          <div className="rounded bg-white p-4 shadow">
            <p className="text-sm text-gray-500">Cancelled Bookings</p>
            <p className="text-2xl font-bold">{stats.cancelledBookings}</p>
          </div>
        </div>
      </div>

      <div className="rounded bg-white p-6 shadow">
        <h2 className="mb-4 text-2xl font-bold">Create Room</h2>

        <form onSubmit={handleCreateRoom} className="grid gap-4 md:grid-cols-2">
          <input
            name="name"
            placeholder="Room name"
            value={roomForm.name}
            onChange={handleRoomChange}
            className="rounded border px-4 py-2"
          />

          <input
            name="pricePerNight"
            type="number"
            placeholder="Price per night"
            value={roomForm.pricePerNight}
            onChange={handleRoomChange}
            className="rounded border px-4 py-2"
          />

          <input
            name="capacity"
            type="number"
            placeholder="Capacity"
            value={roomForm.capacity}
            onChange={handleRoomChange}
            className="rounded border px-4 py-2"
          />

          <input
            name="location"
            placeholder="Location"
            value={roomForm.location}
            onChange={handleRoomChange}
            className="rounded border px-4 py-2"
          />

          <input
            name="address"
            placeholder="Address"
            value={roomForm.address}
            onChange={handleRoomChange}
            className="rounded border px-4 py-2"
          />

          <input
            name="imageUrl"
            placeholder="Image URL"
            value={roomForm.imageUrl}
            onChange={handleRoomChange}
            className="rounded border px-4 py-2 md:col-span-2"
          />

          <textarea
            name="description"
            placeholder="Description"
            value={roomForm.description}
            onChange={handleRoomChange}
            className="rounded border px-4 py-2 md:col-span-2"
            rows="4"
          />

          <input
            name="amenities"
            placeholder="Amenities (comma separated)"
            value={roomForm.amenities}
            onChange={handleRoomChange}
            className="rounded border px-4 py-2 md:col-span-2"
          />

          <button
            type="submit"
            className="rounded bg-blue-600 px-4 py-2 text-white hover:bg-blue-700 md:col-span-2"
          >
            Create Room
          </button>
        </form>
      </div>

      <div>
        <h2 className="mb-4 text-2xl font-bold">All Bookings</h2>

        {bookings.length === 0 ? (
          <p>No bookings found.</p>
        ) : (
          <div className="space-y-4">
            {bookings.map((booking) => (
              <BookingCard
                key={booking.id}
                booking={booking}
                showUser={true}
                onConfirm={confirmBooking}
                onCancel={cancelBooking}
              />
            ))}
          </div>
        )}
      </div>
    </div>
  );
}

export default AdminDashboardPage;