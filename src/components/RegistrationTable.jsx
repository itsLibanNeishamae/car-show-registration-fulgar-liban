
import StatusBadge from "./StatusBadge";
import PaymentBadge from "./PaymentBadge";
import { formatCurrency } from "../api/sample-data";

function RegistrationTable({ registrations, loading, onClearFilters }) {
  return (
    <div className="table-scroll">
      <table className="registrations-table">
        <thead>
          <tr>
            <th>Reg. No.</th>
            <th>Date</th>
            <th>Participant</th>
            <th>Car</th>
            <th>Type</th>
            <th>Status</th>
            <th>Fee / Paid</th>
            <th>Payment</th>
          </tr>
        </thead>

        <tbody>
          {loading ? (
            <tr>
              <td colSpan="8" className="empty-message">
                Loading registrations...
              </td>
            </tr>
          ) : registrations.length === 0 ? (
            <tr>
              <td colSpan="8" className="empty-message">
                No registrations match your filters.

                <button
                  type="button"
                  onClick={onClearFilters}
                >
                  Clear filters
                </button>
              </td>
            </tr>
          ) : (
            registrations.map((item) => (
              <tr key={item.id}>
                <td>{item.registrationNumber}</td>

                <td>{item.registrationDate}</td>

                <td>
                  <div className="participant-name">
                    {item.participantName}
                  </div>
                  <small>{item.contactNumber}</small>
                </td>

                <td>
                  <div>{item.carMakeModel}</div>
                  <small>{item.plateNumber}</small>
                </td>

                <td>{item.registrationType}</td>

                <td>
                  <StatusBadge status={item.status} />
                </td>

                <td>
                  <div>{formatCurrency(item.registrationFee)}</div>
                  <small>
                    Paid: {formatCurrency(item.amountPaid)}
                  </small>
                </td>

                <td>
                  <PaymentBadge
                    paymentStatus={item.paymentStatus}
                  />
                </td>
              </tr>
            ))
          )}
        </tbody>
      </table>
    </div>
  );
}

export default RegistrationTable;
