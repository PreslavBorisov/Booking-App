import { useEffect, useState } from "react";
import api from "../api/axios";
import BookingCard from "../components/BookingCard";

function MyBookingsPage() {
  const [bookings, setBookings] = useState([]);
  const [message, setMessage] = useState("");
  const [error, setError] = useState("");

  const loadBookings = async () => {
    try {
      const response = await api.get("/booking/me");
      setBookings(response.data);
    } catch (err) {
      console.error("Failed to load bookings", err);
    }
  };

  useEffect(() => {
    loadBookings();
  }, []);

  const cancelBooking = async (id) => {
    setMessage("");
    setError("");

    try {
      await api.delete(`/booking/${id}`);
      setMessage("Booking cancelled successfully.");
      loadBookings();
    } catch (err) {
      setError(err.response?.data?.message || "Failed to cancel booking.");
    }
  };

  return (
    <div>
      <h1 className="mb-6 text-3xl font-bold">My Bookings</h1>

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

      {bookings.length === 0 ? (
        <p>You have no bookings yet.</p>
      ) : (
        <div className="space-y-4">
          {bookings.map((booking) => (
            <BookingCard
              key={booking.id}
              booking={booking}
              onCancel={cancelBooking}
            />
          ))}
        </div>
      )}
    </div>
  );
}

export default MyBookingsPage;