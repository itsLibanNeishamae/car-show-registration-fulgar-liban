
function StatusBadge({ status }) {
  const normalizedStatus = status?.toLowerCase();

  const statusLabels = {
    pending: "Pending",
    confirmed: "Confirmed",
    checkedin: "Checked In",
    cancelled: "Cancelled",
  };

  return (
    <span className={`status-badge ${normalizedStatus || ""}`}>
      {statusLabels[normalizedStatus] || status || "Unknown"}
    </span>
  );
}

export default StatusBadge;
