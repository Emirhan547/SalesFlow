import {
  Cell,
  Pie,
  PieChart,
  ResponsiveContainer,
  Tooltip,
} from "recharts";

import Card from "@/components/ui/Card";

import type { DashboardLeadDto } from "../types/DashboardDto";

type Props = {
  leads: DashboardLeadDto;
};

const COLORS = [
  "#2563eb",
  "#22c55e",
  "#eab308",
  "#9333ea",
  "#ef4444",
];

function DashboardLeadChart({ leads }: Props) {

  const data = [
    {
      name: "New",
      value: leads.new,
    },
    {
      name: "Contacted",
      value: leads.contacted,
    },
    {
      name: "Qualified",
      value: leads.qualified,
    },
    {
      name: "Converted",
      value: leads.converted,
    },
    {
      name: "Lost",
      value: leads.lost,
    },
  ];

  return (
    <Card
      title="Lead Distribution"
      subtitle="Current lead statuses"
    >

      <ResponsiveContainer
        width="100%"
        height={360}
      >

        <PieChart>

          <Pie
            data={data}
            innerRadius={75}
            outerRadius={115}
            paddingAngle={4}
            dataKey="value"
          >

            {data.map((_, index) => (

              <Cell
                key={index}
                fill={COLORS[index]}
              />

            ))}

          </Pie>

          <Tooltip />

        </PieChart>

      </ResponsiveContainer>

    </Card>
  );
}

export default DashboardLeadChart;