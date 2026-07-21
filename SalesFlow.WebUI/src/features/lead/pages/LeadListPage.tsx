import { useMemo, useState } from "react";
import { useNavigate } from "react-router-dom";

import {
  Download,
  FileText,
  Plus,
} from "lucide-react";

import { toast } from "sonner";

import { Button } from "@/components/ui/button";

import EmptyState from "@/components/common/EmptyState";
import LoadingState from "@/components/common/LoadingState";
import PageHeader from "@/components/common/PageHeader";

import LeadPagination from "../components/LeadPagination";
import LeadSearch from "../components/LeadSearch";
import LeadTable from "../components/LeadTable";

import {
  exportLeadsExcel,
  exportLeadsPdf,
} from "../services/leadService";

import { useLeads } from "../hooks/useLeads";
import { useDebounce } from "../hooks/useDebounce";

function LeadListPage() {

  const navigate = useNavigate();

  const [search, setSearch] = useState("");

  const [page, setPage] = useState(1);

  const pageSize = 10;

  const debouncedSearch =
    useDebounce(search);

  const filter = useMemo(
    () => ({
      page,
      pageSize,
      search: debouncedSearch,
    }),
    [page, pageSize, debouncedSearch]
  );

  const {
    data,
    loading,
    error,
    reload,
  } = useLeads(filter);

  async function download(
    file: Blob,
    fileName: string
  ) {

    const url =
      window.URL.createObjectURL(file);

    const a =
      document.createElement("a");

    a.href = url;

    a.download = fileName;

    a.click();

    window.URL.revokeObjectURL(url);

  }

  async function handleExcel() {

    try {

      const file =
        await exportLeadsExcel();

      await download(
        file,
        "Leads.xlsx"
      );

      toast.success("Excel exported.");

    }
    catch {

      toast.error("Export failed.");

    }

  }

  async function handlePdf() {

    try {

      const file =
        await exportLeadsPdf();

      await download(
        file,
        "Leads.pdf"
      );

      toast.success("PDF exported.");

    }
    catch {

      toast.error("Export failed.");

    }

  }

  if (loading)
    return <LoadingState />;

  if (error)
    return (
      <div className="rounded-xl border border-red-200 bg-red-50 p-6 text-red-600">
        {error}
      </div>
    );

  if (!data)
    return (
      <EmptyState
        title="No Leads"
        description="There are no leads yet."
      />
    );

  return (

    <div className="space-y-8">

      <PageHeader
        title="Leads"
        description="Manage your sales leads."
        action={

          <div className="flex gap-3">

            <Button
              variant="outline"
              onClick={handlePdf}
            >
              <FileText
                size={18}
                className="mr-2"
              />
              Export PDF
            </Button>

            <Button
              variant="outline"
              onClick={handleExcel}
            >
              <Download
                size={18}
                className="mr-2"
              />
              Export Excel
            </Button>

            <Button
              onClick={() =>
                navigate("/leads/create")
              }
            >
              <Plus
                size={18}
                className="mr-2"
              />
              New Lead
            </Button>

          </div>

        }
      />

      <LeadSearch
        value={search}
        onChange={(value) => {

          setSearch(value);

          setPage(1);

        }}
      />

      <LeadTable
        leads={data.items}
        page={data.page}
        pageSize={pageSize}
        onDeleted={reload}
      />

      <LeadPagination
        page={data.page}
        totalPages={data.totalPages}
        hasPrevious={data.hasPrevious}
        hasNext={data.hasNext}
        onPageChange={setPage}
      />

    </div>

  );

}

export default LeadListPage;