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
    <div className="flex flex-col items-center justify-center py-16 text-center">

      <div className="mb-6 rounded-2xl bg-slate-100 p-4">

        <Inbox
          size={40}
          className="text-slate-400"
        />

      </div>

      <h3 className="text-base font-semibold text-slate-900">

        {title}

      </h3>

      <p className="mt-2 text-sm text-slate-500 max-w-sm">

        {description}

      </p>

    </div>
  );
}

export default EmptyState;