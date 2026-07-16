import { Search } from "lucide-react";

type Props = {
  value: string;
  onChange: (
    value: string
  ) => void;
};

function TaskSearch({
  value,
  onChange,
}: Props) {
  return (
    <div className="relative">

      <Search
        className="absolute left-4 top-1/2 -translate-y-1/2 text-slate-400"
        size={18}
      />

      <input
        value={value}
        onChange={(e) =>
          onChange(e.target.value)
        }
        placeholder="Search tasks..."
        className="h-12 w-full rounded-2xl border border-slate-200 pl-11 pr-4 outline-none focus:border-blue-500"
      />

    </div>
  );
}

export default TaskSearch;