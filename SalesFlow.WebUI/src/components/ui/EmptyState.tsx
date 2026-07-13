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
    <div className="flex flex-col items-center justify-center py-12 text-center">

      <div className="mb-5 rounded-full bg-slate-100 p-5">

        <Inbox
          size={36}
          className="text-slate-400"
        />

      </div>

      <h3 className="text-lg font-semibold">

        {title}

      </h3>

      <p className="mt-2 text-sm text-slate-500">

        {description}

      </p>

    </div>
  );
}

export default EmptyState;