import { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import api from "../api/axios";
import { useAuth } from "../context/AuthContext";

function RoomDetailsPage() {
  const { id } = useParams();
  const { isAuthenticated } = useAuth();

  const [room, setRoom] = useState(null);
  const [loading, setLoading] = useState(true);
  const [bookingForm, setBookingForm] = useState({
    checkIn: "",
    checkOut: "",
  });
  const [availability, setAvailability] = useState(null);
  const [bookingMessage, setBookingMessage] = useState("");
  const [bookingError, setBookingError] = useState("");

  useEffect(() => {
    const loadRoom = async () => {
      try {
        const response = await api.get(`/room/${id}`);
        setRoom(response.data);
      } catch (error) {
        console.error("Failed to load room details", error);
      } finally {
        setLoading(false);
      }
    };

    loadRoom();
  }, [id]);

  const handleChange = (e) => {
    setBookingForm((prev) => ({
      ...prev,
      [e.target.name]: e.target.value,
    }));
  };

  const checkAvailability = async () => {
    setAvailability(null);
    setBookingError("");
    setBookingMessage("");

    if (!bookingForm.checkIn || !bookingForm.checkOut) {
      setBookingError("Please select check-in and check-out dates.");
      return;
    }

    try {
      const response = await api.get(
        `/room/${id}/availability?checkIn=${bookingForm.checkIn}&checkOut=${bookingForm.checkOut}`
      );
      setAvailability(response.data);
    } catch (error) {
      setBookingError(error.response?.data?.message || "Failed to check availability.");
    }
  };

  const createBooking = async () => {
    setBookingError("");
    setBookingMessage("");

    if (!isAuthenticated) {
      setBookingError("You need to be logged in to create a booking.");
      return;
    }

    try {
      const response = await api.post("/booking", {
        roomId: Number(id),
        checkIn: bookingForm.checkIn,
        checkOut: bookingForm.checkOut,
      });

      setBookingMessage(`Booking created successfully. Status: ${response.data.status}`);
      setAvailability(null);
    } catch (error) {
      setBookingError(error.response?.data?.message || "Failed to create booking.");
    }
  };

  if (loading) {
    return <p>Loading room details...</p>;
  }

  if (!room) {
    return <p>Room not found.</p>;
  }

  return (
    <div className="grid gap-8 lg:grid-cols-2">
      <div>
        {room.imageUrl ? (
          <img
            src={room.imageUrl}
            alt={room.name}
            className="h-96 w-full rounded object-cover shadow"
          />
        ) : (
          <div className="flex h-96 items-center justify-center rounded bg-gray-200 text-gray-500 shadow">
            No image
          </div>
        )}
      </div>

      <div className="rounded bg-white p-6 shadow">
        <h1 className="text-3xl font-bold">{room.name}</h1>
        <p className="mt-2 text-gray-600">{room.location}</p>
        <p className="mt-1 text-gray-500">{room.address}</p>

        <p className="mt-4 text-gray-700">{room.description}</p>

        <div className="mt-6 flex items-center gap-6">
          <span className="text-2xl font-bold text-blue-600">
            ${room.pricePerNight} / night
          </span>
          <span className="text-gray-600">{room.capacity} guests</span>
        </div>

        {room.amenities?.length > 0 && (
          <div className="mt-6">
            <h2 className="mb-2 text-lg font-semibold">Amenities</h2>
            <div className="flex flex-wrap gap-2">
              {room.amenities.map((a) => (
                <span
                  key={a}
                  className="rounded-full bg-blue-100 px-3 py-1 text-sm text-blue-700"
                >
                  {a}
                </span>
              ))}
            </div>
          </div>
        )}

        <div className="mt-8 border-t pt-6">
          <h2 className="mb-4 text-xl font-semibold">Book this room</h2>

          <div className="grid gap-4 md:grid-cols-2">
            <div>
              <label className="mb-1 block text-sm font-medium text-gray-700">
                Check-in
              </label>
              <input
                type="date"
                name="checkIn"
                value={bookingForm.checkIn}
                onChange={handleChange}
                className="w-full rounded border px-4 py-2"
              />
            </div>

            <div>
              <label className="mb-1 block text-sm font-medium text-gray-700">
                Check-out
              </label>
              <input
                type="date"
                name="checkOut"
                value={bookingForm.checkOut}
                onChange={handleChange}
                className="w-full rounded border px-4 py-2"
              />
            </div>
          </div>

          <div className="mt-4 flex gap-3">
            <button
              onClick={checkAvailability}
              className="rounded bg-gray-800 px-4 py-2 text-white hover:bg-gray-900"
            >
              Check Availability
            </button>

            <button
              onClick={createBooking}
              className="rounded bg-blue-600 px-4 py-2 text-white hover:bg-blue-700"
            >
              Book Now
            </button>
          </div>

          {availability && (
            <div className="mt-4 rounded bg-green-100 p-3 text-green-700">
              {availability.isAvailable
                ? "Room is available for these dates."
                : "Room is not available for these dates."}
            </div>
          )}

          {bookingMessage && (
            <div className="mt-4 rounded bg-green-100 p-3 text-green-700">
              {bookingMessage}
            </div>
          )}

          {bookingError && (
            <div className="mt-4 rounded bg-red-100 p-3 text-red-700">
              {bookingError}
            </div>
          )}
        </div>
      </div>
    </div>
  );
}

export default RoomDetailsPage;