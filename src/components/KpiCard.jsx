
function KpiCard({ title, value, icon: Icon }) {
  return (
    <div className="kpi-card">
      <div className="kpi-card-top">
        <span>{title}</span>
        <Icon size={22} strokeWidth={1.6} />
      </div>

      <h2>{value}</h2>

      <div className="kpi-line"></div>
    </div>
  );
}

export default KpiCard;
