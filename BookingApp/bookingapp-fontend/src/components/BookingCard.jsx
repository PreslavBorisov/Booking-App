function BookingCard({ booking, onCancel, onConfirm, showUser = false }) {
  const status = String(booking.status || "").trim().toLowerCase();

  return (
    <div className="overflow-hidden rounded bg-white shadow">
      {booking.imageUrl ? (
        <img
          src={booking.imageUrl}
          alt={`Room #${booking.id}`}
          className="h-48 w-full object-cover"
        />
      ) : (
        <div className="flex h-48 w-full items-center justify-center bg-gray-200 text-gray-500">
          No image
        </div>
      )}

      <div className="p-4">
        <h2 className="text-lg font-semibold text-gray-800">
            {booking.roomName}
        </h2>

        <p className="text-sm text-gray-500">
            {booking.checkIn} → {booking.checkOut}
        </p>

        <p className="mt-2 text-sm">
          Status:{" "}
          <span className="font-medium text-blue-600">{booking.status}</span>
        </p>

        {booking.totalPrice && (
          <p className="mt-1 text-sm text-gray-600">
            Total: ${booking.totalPrice}
          </p>
        )}

        <div className="mt-3 flex gap-2">
          {onConfirm && status === "pending" && (
            <button
              onClick={() => onConfirm(booking.id)}
              className="rounded bg-green-600 px-3 py-1 text-white hover:bg-green-700"
            >
              Confirm
            </button>
          )}

          {onCancel && status !== "cancelled" && (
            <button
              onClick={() => onCancel(booking.id)}
              className="rounded bg-red-500 px-3 py-1 text-white hover:bg-red-600"
            >
              Cancel
            </button>
          )}
        </div>
      </div>
    </div>
  );
}

export default BookingCard;