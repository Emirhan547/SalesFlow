import { Search } from "lucide-react";

import { Input } from "@/components/ui/input";

type Props = {
  value: string;
  onChange: (value: string) => void;
};

function NoteSearch({
  value,
  onChange,
}: Props) {
  return (
    <div className="relative">

      <Search
        size={18}
        className="absolute left-3 top-1/2 -translate-y-1/2 text-slate-400"
      />

      <Input
        value={value}
        placeholder="Search notes..."
        className="pl-10"
        onChange={(e) =>
          onChange(e.target.value)
        }
      />

    </div>
  );
}

export default NoteSearch;