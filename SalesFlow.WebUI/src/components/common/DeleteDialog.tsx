import { AlertCircle } from "lucide-react";

type Props = {
  open: boolean;
  onOpenChange: (open: boolean) => void;
  title: string;
  description: string;
  onConfirm: () => void | Promise<void>;
};

function DeleteDialog({
  open,
  onOpenChange,
  title,
  description,
  onConfirm,
}: Props) {
  if (!open) return null;

  async function handleConfirm() {
    await onConfirm();
    onOpenChange(false);
  }

  return (
    <div className="fixed inset-0 z-50 flex items-center justify-center bg-black/30 backdrop-blur-sm">
      <div className="w-full max-w-md rounded-xl bg-white p-6 shadow-lg">

        <div className="flex items-start gap-4">

          <div className="flex-shrink-0">
            <AlertCircle className="h-6 w-6 text-red-500" />
          </div>

          <div className="flex-1">

            <h2 className="text-lg font-semibold text-slate-900">
              {title}
            </h2>

            <p className="mt-2 text-sm text-slate-600">
              {description}
            </p>

          </div>

        </div>

        <div className="mt-6 flex justify-end gap-3">

          <button
            onClick={() => onOpenChange(false)}
            className="rounded-lg border border-slate-300 bg-white px-4 py-2 text-sm font-medium text-slate-700 transition-all hover:bg-slate-50"
          >
            Cancel
          </button>

          <button
            onClick={handleConfirm}
            className="rounded-lg bg-red-600 px-4 py-2 text-sm font-medium text-white transition-all hover:bg-red-700"
          >
            Delete
          </button>

        </div>

      </div>
    </div>
  );
}

export default DeleteDialog;