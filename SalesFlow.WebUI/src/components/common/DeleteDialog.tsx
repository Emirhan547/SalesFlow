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
    <div className="fixed inset-0 z-50 flex items-center justify-center bg-black/50">
      <div className="w-full max-w-md rounded-xl bg-white p-6 shadow-xl">

        <h2 className="text-xl font-semibold">
          {title}
        </h2>

        <p className="mt-2 text-sm text-slate-600">
          {description}
        </p>

        <div className="mt-6 flex justify-end gap-3">

          <button
            onClick={() => onOpenChange(false)}
            className="rounded-lg border px-4 py-2 hover:bg-slate-100"
          >
            Cancel
          </button>

          <button
            onClick={handleConfirm}
            className="rounded-lg bg-red-600 px-4 py-2 text-white hover:bg-red-700"
          >
            Delete
          </button>

        </div>

      </div>
    </div>
  );
}

export default DeleteDialog;