import { useCustomers } from "../hooks/useCustomers";
import CustomerPagination from "./CustomerPagination";
import CustomerRow from "./CustomerRow";
import CustomerToolbar from "./CustomerToolbar";

import type { Customer } from "@/features/customer/types/Customer";

function CustomerTable() {

  const {
    customers,
    loading,
    page,
    setPage,
    search,
    setSearch,
  } = useCustomers();

  if (loading) {
    return <h2>Loading...</h2>;
  }

  return (
    <>

      <CustomerToolbar
        search={search}
        onSearchChange={setSearch}
        onCreate={() => {}}
      />

      <table className="w-full bg-white rounded-xl overflow-hidden">

        <thead>

          <tr className="bg-zinc-100">

            <th className="p-3">Company</th>

            <th className="p-3">Contact</th>

            <th className="p-3">Email</th>

            <th className="p-3">Phone</th>

            <th className="p-3">Type</th>

            <th className="p-3">Actions</th>

          </tr>

        </thead>

        <tbody>

          {customers?.items.map((customer: Customer) => (

            <CustomerRow
              key={customer.id}
              customer={customer}
            />

          ))}

        </tbody>

      </table>

      <CustomerPagination
        page={page}
        totalPages={customers?.totalPages ?? 1}
        onChange={setPage}
      />

    </>
  );
}

export default CustomerTable;