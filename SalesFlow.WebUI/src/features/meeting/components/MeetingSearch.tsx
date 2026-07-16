import { Search } from "lucide-react";

type Props = {
  value: string;
  onChange: (
    value: string
  ) => void;
};

function MeetingSearch({
  value,
  onChange,
}: Props) {
  return (
    <div className="rounded-2xl bg-white p-6 shadow">

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
          placeholder="Search meetings..."
          className="h-12 w-full rounded-xl border border-slate-200 pl-11 pr-4 outline-none focus:border-blue-500"
        />

      </div>

    </div>
  );
}

export default MeetingSearch;