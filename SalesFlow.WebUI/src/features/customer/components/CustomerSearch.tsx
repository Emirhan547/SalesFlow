import { Input } from "@/components/ui/input";
import type { ChangeEvent } from "react";

type Props = {
  value: string;
  onChange: (value: string) => void;
};

function CustomerSearch({
  value,
  onChange,
}: Props) {
  return (
    <div className="rounded-3xl border border-slate-200 bg-white p-4 shadow-sm">
      <label className="mb-3 block text-sm font-medium text-slate-700">
        Search customers
      </label>
      <Input
        placeholder="Search by company, contact name or email"
        value={value}
        onChange={(event: ChangeEvent<HTMLInputElement>) =>
          onChange(event.target.value)
        }
      />
    </div>
  );
}

export default CustomerSearch;