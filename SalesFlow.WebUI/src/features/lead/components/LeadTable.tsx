import { useState } from "react";
import { useNavigate } from "react-router-dom";

import {
  Building2,
  Eye,
  Mail,
  Pencil,
  Phone,
  Trash2,
  User,
} from "lucide-react";

import Card from "@/components/ui/Card";
import ConfirmDialog from "@/components/ui/ConfirmDialog";

import { useDeleteLead } from "../hooks/useDeleteLead";
import type { Lead } from "../types/Lead";


type Props = {
  leads: Lead[];
  onDeleted?: () => void;
};

function getStatusBadge(status: number) {
  switch (status) {
    case 1:
      return "bg-blue-100 text-blue-700";
    case 2:
      return "bg-amber-100 text-amber-700";
    case 3:
      return "bg-green-100 text-green-700";
    case 4:
      return "bg-red-100 text-red-700";
    case 5:
      return "bg-purple-100 text-purple-700";
    default:
      return "bg-slate-100 text-slate-700";
  }
}

function getStatusText(status: number) {
  switch (status) {
    case 1:
      return "New";
    case 2:
      return "Contacted";
    case 3:
      return "Qualified";
    case 4:
      return "Lost";
    case 5:
      return "Converted";
    default:
      return "-";
  }
}

function getSourceText(source: number) {
  switch (source) {
    case 1:
      return "Website";
    case 2:
      return "Phone";
    case 3:
      return "Email";
    case 4:
      return "Referral";
    case 5:
      return "Social";
    case 6:
      return "Advertisement";
    case 7:
      return "Other";
    default:
      return "-";
  }
}

function LeadTable({
  leads,
  onDeleted,
}: Props) {
  const navigate = useNavigate();

  const [selectedId, setSelectedId] =
    useState<number>();

  const [dialogOpen, setDialogOpen] =
    useState(false);

  const {
    loading,
    remove,
  } = useDeleteLead();

  if (leads.length === 0) {
    return (
      <Card title="Leads">
        <div className="flex h-72 items-center justify-center text-slate-400">
          No leads found.
        </div>
      </Card>
    );
  }

  return (
    <>
      <Card
        title="Leads"
        subtitle={`${leads.length} lead(s)`}
      >
        <div className="overflow-x-auto">

          <table className="w-full">

            <thead>

              <tr className="border-b border-slate-200 text-left text-sm font-semibold text-slate-500">

                <th className="pb-4">
                  Lead
                </th>

                <th className="pb-4">
                  Company
                </th>

                <th className="pb-4">
                  Contact
                </th>

                <th className="pb-4">
                  Status
                </th>

                <th className="pb-4">
                  Source
                </th>

                <th className="pb-4 text-right">
                  Actions
                </th>

              </tr>

            </thead>

            <tbody>

              {leads.map((lead) => (

                <tr
                  key={lead.id}
                  className="border-b border-slate-100 transition hover:bg-slate-50"
                >

                  <td className="py-5">

                    <div className="flex items-center gap-4">

                      <div className="flex h-12 w-12 items-center justify-center rounded-2xl bg-blue-100 text-blue-600">

                        <User size={20} />

                      </div>

                      <div>

                        <h3 className="font-semibold">

                          {lead.firstName} {lead.lastName}

                        </h3>

                        <p className="text-sm text-slate-500">

                          Lead #{lead.id}

                        </p>

                      </div>

                    </div>

                  </td>

                  <td>

                    <div className="flex items-center gap-2">

                      <Building2 size={16} />

                      {lead.companyName || "-"}

                    </div>

                  </td>

                  <td>

                    <div className="space-y-1">

                      <div className="flex items-center gap-2">

                        <Mail size={15} />

                        {lead.email}

                      </div>

                      <div className="flex items-center gap-2">

                        <Phone size={15} />

                        {lead.phoneNumber}

                      </div>

                    </div>

                  </td>

                  <td>

                    <span
                      className={`rounded-full px-3 py-1 text-xs font-semibold ${getStatusBadge(
                        lead.status
                      )}`}
                    >
                      {getStatusText(lead.status)}
                    </span>

                  </td>

                  <td>

                    {getSourceText(
                      lead.source
                    )}

                  </td>

                  <td>

                    <div className="flex justify-end gap-2">

                      <button
                        onClick={() =>
                          navigate(`/leads/${lead.id}`)
                        }
                        className="rounded-xl p-2 hover:bg-blue-100"
                      >
                        <Eye size={18} />
                      </button>

                      <button
                        onClick={() =>
                          navigate(`/leads/edit/${lead.id}`)
                        }
                        className="rounded-xl p-2 hover:bg-amber-100"
                      >
                        <Pencil size={18} />
                      </button>

                      <button
                        onClick={() => {
                          setSelectedId(lead.id);
                          setDialogOpen(true);
                        }}
                        className="rounded-xl p-2 hover:bg-red-100"
                      >
                        <Trash2 size={18} />
                      </button>

                    </div>

                  </td>

                </tr>

              ))}

            </tbody>

          </table>

        </div>

      </Card>

      <ConfirmDialog
        open={dialogOpen}
        title="Delete Lead"
        description="Are you sure you want to delete this lead?"
        loading={loading}
        onCancel={() => {
          setDialogOpen(false);
          setSelectedId(undefined);
        }}
        onConfirm={async () => {

          if (!selectedId)
            return;

          const success =
            await remove(selectedId);

          if (!success)
            return;

          setDialogOpen(false);
          setSelectedId(undefined);

          onDeleted?.();

        }}
      />
    </>
  );
}

export default LeadTable;