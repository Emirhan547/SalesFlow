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

import DealSearch from "../components/DealSearch";
import DealTable from "../components/DealTable";
import DealPagination from "../components/DealPagination";

import {
  exportDealsExcel,
  exportDealsPdf,
} from "../services/dealService";

import { useDeals } from "../hooks/useDeals";
import { useDebounce } from "@/features/lead/hooks/useDebounce";

function DealListPage() {

  const navigate =
    useNavigate();

  const [search, setSearch] =
    useState("");

  const [page, setPage] =
    useState(1);

  const debouncedSearch =
    useDebounce(search);

  const filter = useMemo(
    () => ({
      page,
      pageSize: 10,
      search: debouncedSearch,
    }),
    [page, debouncedSearch]
  );

  const {
    data,
    loading,
    error,
    reload,
  } = useDeals(filter);

  async function handleExportExcel() {

    try {

      const blob =
        await exportDealsExcel();

      const url =
        window.URL.createObjectURL(blob);

      const link =
        document.createElement("a");

      link.href = url;

      link.download =
        "Deals.xlsx";

      link.click();

      window.URL.revokeObjectURL(url);

      toast.success(
        "Excel exported successfully."
      );

    }
    catch {

      toast.error(
        "Excel export failed."
      );

    }

  }

  async function handleExportPdf() {

    try {

      const blob =
        await exportDealsPdf();

      const url =
        window.URL.createObjectURL(blob);

      const link =
        document.createElement("a");

      link.href = url;

      link.download =
        "Deals.pdf";

      link.click();

      window.URL.revokeObjectURL(url);

      toast.success(
        "PDF exported successfully."
      );

    }
    catch {

      toast.error(
        "PDF export failed."
      );

    }

  }

  if (loading)
    return <LoadingState />;

  if (error)
    return (
      <div className="rounded-2xl border border-red-200 bg-red-50 p-6 text-red-600">
        {error}
      </div>
    );

  if (!data)
    return (
      <EmptyState
        title="No Deals"
        description="There are no deals yet."
      />
    );

  return (
    <div className="space-y-8">

      <PageHeader
        title="Deals"
        description="Manage all deals."
        action={

          <div className="flex flex-wrap gap-3">

            <Button
              variant="outline"
              onClick={handleExportPdf}
            >

              <FileText
                size={18}
                className="mr-2"
              />

              Export PDF

            </Button>

            <Button
              variant="outline"
              onClick={handleExportExcel}
            >

              <Download
                size={18}
                className="mr-2"
              />

              Export Excel

            </Button>

            <Button
              onClick={() =>
                navigate("/deals/create")
              }
            >

              <Plus
                size={18}
                className="mr-2"
              />

              New Deal

            </Button>

          </div>

        }
      />

      <DealSearch
        value={search}
        onChange={(value) => {

          setSearch(value);

          setPage(1);

        }}
      />

      <DealTable
        deals={data.items}
        onDeleted={reload}
      />

      <DealPagination
        page={data.page}
        totalPages={data.totalPages}
        hasPrevious={data.hasPrevious}
        hasNext={data.hasNext}
        onPageChange={setPage}
      />

    </div>
  );
}

export default DealListPage;