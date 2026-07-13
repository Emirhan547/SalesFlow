import type { Customer } from "@/features/customer/types/Customer";

type Props = {
  customer: Customer;
};

function CustomerRow({ customer }: Props) {
  return (
    <tr className="border-b">

      <td className="p-3">
        {customer.companyName}
      </td>

      <td className="p-3">
        {customer.contactFirstName} {customer.contactLastName}
      </td>

      <td className="p-3">
        {customer.email}
      </td>

      <td className="p-3">
        {customer.phoneNumber}
      </td>

      <td className="p-3">
        {customer.customerType}
      </td>

      <td className="p-3">

        <button className="mr-2 text-blue-600">
          Edit
        </button>

        <button className="text-red-600">
          Delete
        </button>

      </td>

    </tr>
  );
}

export default CustomerRow;