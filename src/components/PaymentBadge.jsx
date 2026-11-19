
function PaymentBadge({ paymentStatus }) {
  const normalizedStatus = paymentStatus?.toLowerCase();

  const paymentLabels = {
    paid: "Paid",
    partial: "Partial",
    unpaid: "Unpaid",
  };

  return (
    <span className={`payment-badge ${normalizedStatus || ""}`}>
      {paymentLabels[normalizedStatus] || paymentStatus || "Unknown"}
    </span>
  );
}

export default PaymentBadge;
