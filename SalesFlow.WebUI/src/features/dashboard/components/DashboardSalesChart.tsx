import {
  Area,
  AreaChart,
  CartesianGrid,
  ResponsiveContainer,
  Tooltip,
  XAxis,
  YAxis,
} from "recharts";

import Card from "@/components/ui/Card";

import type { DashboardSalesDto } from "../types/DashboardDto";

type Props = {
  sales: DashboardSalesDto;
};

function DashboardSalesChart({ sales }: Props) {
  const data = [
    {
      name: "Pipeline",
      value: Number(sales.pipelineAmount),
    },
    {
      name: "Won",
      value: Number(sales.wonAmount),
    },
  ];

  return (
    <Card
      title="Sales Overview"
      subtitle="Current sales pipeline"
    >
      <ResponsiveContainer
        width="100%"
        height={360}
      >
        <AreaChart data={data}>

          <defs>

            <linearGradient
              id="sales"
              x1="0"
              y1="0"
              x2="0"
              y2="1"
            >

              <stop
                offset="5%"
                stopColor="#2563eb"
                stopOpacity={0.9}
              />

              <stop
                offset="95%"
                stopColor="#2563eb"
                stopOpacity={0.05}
              />

            </linearGradient>

          </defs>

          <CartesianGrid
            strokeDasharray="4 4"
            stroke="#e2e8f0"
          />

          <XAxis
            dataKey="name"
            tick={{ fill: "#64748b" }}
          />

          <YAxis tick={{ fill: "#64748b" }} />

          <Tooltip />

          <Area
            type="monotone"
            dataKey="value"
            stroke="#2563eb"
            strokeWidth={3}
            fill="url(#sales)"
          />

        </AreaChart>
      </ResponsiveContainer>
    </Card>
  );
}

export default DashboardSalesChart;