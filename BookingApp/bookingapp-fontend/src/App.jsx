import { Link, Route, Routes } from "react-router-dom";
import HomePage from "./pages/HomePage";
import LoginPage from "./pages/LoginPage";
import RegisterPage from "./pages/RegisterPage";
import MyBookingsPage from "./pages/MyBookingsPage";
import AdminDashboardPage from "./pages/AdminDashboardPage";
import RoomDetailsPage from "./pages/RoomDetailsPage";
import ProtectedRoute from "./components/ProtectedRoute";
import { useAuth } from "./context/AuthContext";

function App() {
  const { isAuthenticated, isAdmin, logout, userEmail } = useAuth();

  return (
    <div className="min-h-screen bg-gray-100">
      <nav className="bg-white shadow">
        <div className="mx-auto flex max-w-6xl items-center justify-between px-6 py-4">
          <Link to="/" className="text-xl font-bold text-blue-600">
            Booking App
          </Link>

          <div className="flex items-center gap-4">
            <Link to="/" className="text-sm font-medium text-gray-700 hover:text-blue-600">
              Rooms
            </Link>

            {isAuthenticated && (
              <Link to="/my-bookings" className="text-sm font-medium text-gray-700 hover:text-blue-600">
                My Bookings
              </Link>
            )}

            {isAdmin && (
              <Link to="/admin" className="text-sm font-medium text-gray-700 hover:text-blue-600">
                Admin
              </Link>
            )}

            {!isAuthenticated ? (
              <>
                <Link to="/login" className="text-sm font-medium text-gray-700 hover:text-blue-600">
                  Login
                </Link>
                <Link to="/register" className="rounded bg-blue-600 px-4 py-2 text-sm font-medium text-white hover:bg-blue-700">
                  Register
                </Link>
              </>
            ) : (
              <div className="flex items-center gap-3">
                <span className="text-sm text-gray-600">{userEmail}</span>
                <button
                  onClick={logout}
                  className="rounded bg-red-500 px-4 py-2 text-sm font-medium text-white hover:bg-red-600"
                >
                  Logout
                </button>
              </div>
            )}
          </div>
        </div>
      </nav>

      <main className="mx-auto max-w-6xl px-6 py-8">
        <Routes>
          <Route path="/" element={<HomePage />} />
          <Route path="/rooms/:id" element={<RoomDetailsPage />} />
          <Route path="/login" element={<LoginPage />} />
          <Route path="/register" element={<RegisterPage />} />

          <Route
            path="/my-bookings"
            element={
              <ProtectedRoute>
                <MyBookingsPage />
              </ProtectedRoute>
            }
          />

          <Route
            path="/admin"
            element={
              <ProtectedRoute adminOnly={true}>
                <AdminDashboardPage />
              </ProtectedRoute>
            }
          />
        </Routes>
      </main>
    </div>
  );
}

export default App;