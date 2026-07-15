import { useNavigate } from "react-router-dom";

import { toast } from "sonner";

import PageHeader from "@/components/common/PageHeader";

import CustomerForm from "../components/CustomerForm";

import { createCustomer } from "../services/customerService";

import type { CustomerFormData } from "../schemas/customerSchema";

function CustomerCreatePage() {

  const navigate =
    useNavigate();

  async function handleCreate(
    data: CustomerFormData
  ) {
    try {
      const response =
        await createCustomer(data);

      if (!response.isSuccess) {
        toast.error(response.message);
        return;
      }

      toast.success(response.message);
      navigate("/customers");
    }
    catch {
      toast.error(
        "Customer could not be created. Please check your data and try again."
      );
    }
  }

  return (
    <div className="space-y-8">

      <PageHeader
        title="Create Customer"
        description="Add a new customer to CRM"
      />

      <div className="rounded-3xl border border-slate-200 bg-white p-8 shadow-sm">
        <CustomerForm
          submitText="Create Customer"
          onSubmit={handleCreate}
        />
      </div>

    </div>
  );
}

export default CustomerCreatePage;