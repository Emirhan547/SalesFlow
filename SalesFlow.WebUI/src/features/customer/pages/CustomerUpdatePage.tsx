import { useNavigate, useParams } from "react-router-dom";

import { toast } from "sonner";

import LoadingState from "@/components/common/LoadingState";
import PageHeader from "@/components/common/PageHeader";

import CustomerForm from "../components/CustomerForm";

import { useCustomer } from "../hooks/useCustomer";

import { updateCustomer } from "../services/customerService";

import type { CustomerFormData } from "../schemas/customerSchema";

function CustomerUpdatePage() {

  const { id } = useParams();

  const navigate = useNavigate();

  const {
    customer,
    loading,
    error,
  } = useCustomer(Number(id));

  async function handleUpdate(
    data: CustomerFormData
  ) {
    if (!customer)
      return;

    try {
      const response =
        await updateCustomer({
          id: customer.id,
          ...data,
        });

      if (!response.isSuccess) {
        toast.error(response.message);
        return;
      }

      toast.success(response.message);
      navigate("/customers");
    }
    catch {
      toast.error(
        "Customer could not be updated. Please check your data and try again."
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

  if (!customer)
    return (
      <div className="rounded-2xl bg-white p-8 text-center shadow">
        Customer not found.
      </div>
    );

  return (
    <div className="space-y-8">

      <PageHeader
        title="Update Customer"
        description="Update customer information"
      />

      <div className="rounded-3xl border border-slate-200 bg-white p-8 shadow-sm">
        <CustomerForm
          submitText="Update Customer"
          defaultValues={{
            customerType: customer.customerType,
            companyName: customer.companyName ?? "",
            contactFirstName: customer.contactFirstName,
            contactLastName: customer.contactLastName,
            email: customer.email,
            phoneNumber: customer.phoneNumber,
            website: customer.website ?? "",
            taxNumber: customer.taxNumber ?? "",
            address: customer.address ?? "",
            description: customer.description ?? "",
            assignedUserId: customer.assignedUserId,
          }}
          onSubmit={handleUpdate}
        />
      </div>

    </div>
  );
}

export default CustomerUpdatePage;