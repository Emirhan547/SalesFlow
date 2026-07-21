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

import CustomerPagination from "../components/CustomerPagination";
import CustomerSearch from "../components/CustomerSearch";
import CustomerTable from "../components/CustomerTable";

import {
  exportCustomersExcel,
  exportCustomersPdf,
} from "../services/customerService";

import { useCustomers } from "../hooks/useCustomers";
import { useDebounce } from "../hooks/useDebounce";

function CustomerListPage() {

  const navigate =
    useNavigate();

  const [search, setSearch] =
    useState("");

  const [page, setPage] =
    useState(1);

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
  } = useCustomers(filter);

  async function handleExportExcel() {
    try {

      const blob =
        await exportCustomersExcel();

      const url =
        window.URL.createObjectURL(blob);

      const link =
        document.createElement("a");

      link.href = url;

      link.download =
        "Customers.xlsx";

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
        await exportCustomersPdf();

      const url =
        window.URL.createObjectURL(blob);

      const link =
        document.createElement("a");

      link.href = url;

      link.download =
        "Customers.pdf";

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
        title="No Customers"
        description="There are no customers yet."
      />
    );

  return (
    <div className="space-y-8">

      <PageHeader
        title="Customers"
        description="Manage your customers and customer relationships."
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
                navigate("/customers/create")
              }
            >

              <Plus
                size={18}
                className="mr-2"
              />

              New Customer

            </Button>

          </div>
        }
      />

      <CustomerSearch
        value={search}
        onChange={(value) => {

          setSearch(value);

          setPage(1);

        }}
      />

      <CustomerTable
        customers={data.items}
        page={data.page}
        pageSize={pageSize}
        onDeleted={reload}
      />

      <CustomerPagination
        page={data.page}
        totalPages={data.totalPages}
        hasPrevious={data.hasPrevious}
        hasNext={data.hasNext}
        onPageChange={setPage}
      />

    </div>
  );
}

export default CustomerListPage;