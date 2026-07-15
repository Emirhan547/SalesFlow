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
    <div className="flex h-72 flex-col items-center justify-center rounded-3xl border-2 border-dashed border-slate-200">

      <Inbox
        size={52}
        className="text-slate-300"
      />

      <h3 className="mt-5 text-xl font-semibold">
        {title}
      </h3>

      <p className="mt-2 text-slate-500">
        {description}
      </p>

    </div>
  );
}

export default EmptyState;