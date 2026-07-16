import { Search } from "lucide-react";

type Props = {
  value: string;
  onChange: (value: string) => void;
};

function LeadSearch({
  value,
  onChange,
}: Props) {
  return (
    <div className="relative">

      <Search
        size={18}
        className="absolute left-4 top-1/2 -translate-y-1/2 text-slate-400"
      />

      <input
        value={value}
        onChange={(e) =>
          onChange(e.target.value)
        }
        placeholder="Search lead..."
        className="h-12 w-full rounded-2xl border border-slate-200 bg-white pl-11 pr-4 outline-none transition focus:border-blue-500"
      />

    </div>
  );
}

export default LeadSearch;