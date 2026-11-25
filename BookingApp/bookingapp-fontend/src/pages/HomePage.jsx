import { Link } from "react-router-dom";
import { useEffect, useState } from "react";
import api from "../api/axios";

function HomePage() {
  const [rooms, setRooms] = useState([]);
  const [loading, setLoading] = useState(true);

  const [filters, setFilters] = useState({
    search: "",
    capacity: "",
    maxPrice: "",
    amenity: "",
    sortBy: "",
    sortOrder: "asc",
  });

  const loadRooms = async () => {
    try {
      const params = new URLSearchParams();

      if (filters.search) params.append("search", filters.search);
      if (filters.capacity) params.append("capacity", filters.capacity);
      if (filters.maxPrice) params.append("maxPrice", filters.maxPrice);
      if (filters.amenity) params.append("amenity", filters.amenity);
      if (filters.sortBy) params.append("sortBy", filters.sortBy);
      if (filters.sortOrder) params.append("sortOrder", filters.sortOrder);

      params.append("page", "1");
      params.append("pageSize", "12");

      const response = await api.get(`/room?${params.toString()}`);
      setRooms(response.data.items || []);
    } catch (error) {
      console.error("Failed to load rooms", error);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    loadRooms();
  }, []);

  const handleChange = (e) => {
    setFilters((prev) => ({
      ...prev,
      [e.target.name]: e.target.value,
    }));
  };

  const handleSearch = async (e) => {
    e.preventDefault();
    setLoading(true);
    await loadRooms();
  };

  const handleReset = async () => {
    const resetFilters = {
      search: "",
      capacity: "",
      maxPrice: "",
      amenity: "",
      sortBy: "",
      sortOrder: "asc",
    };

    setFilters(resetFilters);

    try {
      const response = await api.get("/room?page=1&pageSize=12");
      setRooms(response.data.items || []);
    } catch (error) {
      console.error("Failed to reset rooms", error);
    }
  };

  if (loading) {
    return <p>Loading rooms...</p>;
  }

  return (
    <div>
      <h1 className="mb-6 text-3xl font-bold">Available Rooms</h1>

      <form
        onSubmit={handleSearch}
        className="mb-8 grid gap-4 rounded bg-white p-4 shadow md:grid-cols-2 lg:grid-cols-3"
      >
        <input
          name="search"
          placeholder="Search by name, description, location..."
          value={filters.search}
          onChange={handleChange}
          className="rounded border px-4 py-2"
        />

        <input
          name="capacity"
          type="number"
          placeholder="Minimum capacity"
          value={filters.capacity}
          onChange={handleChange}
          className="rounded border px-4 py-2"
        />

        <input
          name="maxPrice"
          type="number"
          placeholder="Max price"
          value={filters.maxPrice}
          onChange={handleChange}
          className="rounded border px-4 py-2"
        />

        <input
          name="amenity"
          placeholder="Amenity (e.g. WiFi)"
          value={filters.amenity}
          onChange={handleChange}
          className="rounded border px-4 py-2"
        />

        <select
          name="sortBy"
          value={filters.sortBy}
          onChange={handleChange}
          className="rounded border px-4 py-2"
        >
          <option value="">Sort by</option>
          <option value="price">Price</option>
          <option value="capacity">Capacity</option>
          <option value="newest">Newest</option>
        </select>

        <select
          name="sortOrder"
          value={filters.sortOrder}
          onChange={handleChange}
          className="rounded border px-4 py-2"
        >
          <option value="asc">Ascending</option>
          <option value="desc">Descending</option>
        </select>

        <div className="flex gap-3 md:col-span-2 lg:col-span-3">
          <button
            type="submit"
            className="rounded bg-blue-600 px-4 py-2 text-white hover:bg-blue-700"
          >
            Search
          </button>

          <button
            type="button"
            onClick={handleReset}
            className="rounded bg-gray-300 px-4 py-2 text-gray-800 hover:bg-gray-400"
          >
            Reset
          </button>
        </div>
      </form>

      <div className="grid gap-6 md:grid-cols-2 lg:grid-cols-3">
        {rooms.map((room) => (
          <div key={room.id} className="overflow-hidden rounded bg-white shadow">
            {room.imageUrl ? (
              <img
                src={room.imageUrl}
                alt={room.name}
                className="h-48 w-full object-cover"
              />
            ) : (
              <div className="flex h-48 items-center justify-center bg-gray-200 text-gray-500">
                No image
              </div>
            )}

            <div className="p-4">
              <h2 className="text-xl font-semibold">{room.name}</h2>
              <p className="mt-1 text-sm text-gray-600">{room.location}</p>
              <p className="mt-2 text-sm text-gray-700">{room.description}</p>

              <div className="mt-4 flex items-center justify-between">
                <span className="font-bold text-blue-600">
                  ${room.pricePerNight} / night
                </span>
                <span className="text-sm text-gray-500">
                  {room.capacity} guests
                </span>
              </div>

              {room.amenities?.length > 0 && (
                <div className="mt-3 flex flex-wrap gap-2">
                  {room.amenities.map((a) => (
                    <span
                      key={a}
                      className="rounded-full bg-blue-100 px-3 py-1 text-xs text-blue-700"
                    >
                      {a}
                    </span>
                  ))}
                </div>
              )}

              <Link
                to={`/rooms/${room.id}`}
                className="mt-4 inline-block rounded bg-blue-600 px-4 py-2 text-sm font-medium text-white hover:bg-blue-700"
              >
                View Details
              </Link>
            </div>
          </div>
        ))}
      </div>
    </div>
  );
}

export default HomePage;