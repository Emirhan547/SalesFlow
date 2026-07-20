import { Inbox } from "lucide-react";

type Props = {
  title: string;
  description: string;
};

function EmptyState({
  title,
  description,
}: Props) {
  return (
    <div className="flex h-72 flex-col items-center justify-center rounded-xl border border-dashed border-slate-300 bg-slate-50">

      <Inbox
        size={48}
        className="text-slate-300"
      />

      <h3 className="mt-5 text-lg font-semibold text-slate-900">
        {title}
      </h3>

      <p className="mt-2 text-sm text-slate-500 max-w-xs text-center">
        {description}
      </p>

    </div>
  );
}

export default EmptyState;