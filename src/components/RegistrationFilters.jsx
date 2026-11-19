
import {
  Search,
  ChevronDown,
} from "lucide-react";

function RegistrationFilters({
  participantName,
  setParticipantName,
  type,
  setType,
  status,
  setStatus,
}) {
  const clearFilters = () => {
    setParticipantName("");
    setType("");
    setStatus("");
  };

  return (
    <div className="filter-controls">
      <div className="search-box">
        <Search size={17} />

        <input
          type="text"
          placeholder="Search participant name..."
          value={participantName}
          onChange={(e) => setParticipantName(e.target.value)}
        />
      </div>

      <div className="select-box">
        <select
          value={type}
          onChange={(e) => setType(e.target.value)}
        >
          <option value="">Type</option>
          <option value="Exhibitor">Exhibitor</option>
          <option value="Competitor">Competitor</option>
          <option value="VIP">VIP</option>
        </select>

        <ChevronDown size={14} />
      </div>

      <div className="select-box">
        <select
          value={status}
          onChange={(e) => setStatus(e.target.value)}
        >
          <option value="">Status</option>
          <option value="Pending">Pending</option>
          <option value="Confirmed">Confirmed</option>
          <option value="CheckedIn">Checked In</option>
          <option value="Cancelled">Cancelled</option>
        </select>

        <ChevronDown size={14} />
      </div>

      <button
        type="button"
        className="clear-button"
        onClick={clearFilters}
      >
        Clear
      </button>
    </div>
  );
}

export default RegistrationFilters;
