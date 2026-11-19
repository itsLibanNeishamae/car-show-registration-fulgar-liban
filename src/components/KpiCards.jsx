
import {
  Users,
  Wallet,
  HandCoins,
  BadgeCheck,
  ClipboardCheck,
} from "lucide-react";

import KpiCard from "./KpiCard";
import { formatCurrency } from "../api/sample-data";

function KpiCards({ summary }) {
  const kpiCards = [
    {
      title: "Total Registrations",
      value: summary.totalRegistrations,
      icon: Users,
    },
    {
      title: "Total Payments",
      value: formatCurrency(summary.totalPayments),
      icon: Wallet,
    },
    {
      title: "Uncollected Payments",
      value: formatCurrency(summary.uncollectedPayments),
      icon: HandCoins,
    },
    {
      title: "Confirmed",
      value: summary.confirmed,
      icon: BadgeCheck,
    },
    {
      title: "Checked In",
      value: summary.checkedIn,
      icon: ClipboardCheck,
    },
  ];

  return (
    <section className="kpi-section">
      <div className="kpi-grid">
        {kpiCards.map((card) => (
          <KpiCard
            key={card.title}
            title={card.title}
            value={card.value}
            icon={card.icon}
          />
        ))}
      </div>
    </section>
  );
}

export default KpiCards;
