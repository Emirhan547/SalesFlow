type Props = {
  open: boolean;

  title: string;

  description: string;

  loading?: boolean;

  onConfirm: () => void;

  onCancel: () => void;
};

function ConfirmDialog({

  open,

  title,

  description,

  loading,

  onConfirm,

  onCancel,

}: Props) {

  if (!open)
    return null;

  return (

    <div className="fixed inset-0 z-50 flex items-center justify-center bg-black/40">

      <div className="w-full max-w-md rounded-3xl bg-white p-8 shadow-2xl">

        <h2 className="text-2xl font-bold">

          {title}

        </h2>

        <p className="mt-3 text-slate-500">

          {description}

        </p>

        <div className="mt-8 flex justify-end gap-3">

          <button
            onClick={onCancel}
            className="rounded-xl border px-5 py-2"
          >

            Cancel

          </button>

          <button
            disabled={loading}
            onClick={onConfirm}
            className="rounded-xl bg-red-600 px-5 py-2 font-medium text-white"
          >

            Delete

          </button>

        </div>

      </div>

    </div>

  );

}

export default ConfirmDialog;