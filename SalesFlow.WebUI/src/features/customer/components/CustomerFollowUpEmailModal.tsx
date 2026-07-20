import { useState } from "react";

import { useGenerateFollowUpEmail } from "../hooks/useGenerateFollowUpEmail";

type Props = {
  open: boolean;
  customerId: number;
  onClose: () => void;
};

function CustomerFollowUpEmailModal({
  open,
  customerId,
  onClose,
}: Props) {

  const [tone, setTone] =
    useState("Professional");

  const [purpose, setPurpose] =
    useState("Follow Up");

  const {

    email,

    loading,

    error,

    generate,

    clear,

  } = useGenerateFollowUpEmail();

  if (!open)
    return null;

  const handleClose = () => {

    clear();

    setTone("Professional");

    setPurpose("Follow Up");

    onClose();

  };

  return (

    <div className="fixed inset-0 z-50 flex items-center justify-center bg-black/40">

      <div className="w-full max-w-2xl rounded-3xl bg-white p-8 shadow-2xl">

        <h2 className="text-2xl font-bold">

          Generate Follow-up Email

        </h2>

        <div className="mt-6 grid gap-6 md:grid-cols-2">

          <div>

            <label className="mb-2 block text-sm font-medium">

              Tone

            </label>

            <select
              value={tone}
              onChange={(e) => setTone(e.target.value)}
              className="w-full rounded-xl border p-3"
            >

              <option>Professional</option>
              <option>Friendly</option>
              <option>Formal</option>
              <option>Casual</option>

            </select>

          </div>

          <div>

            <label className="mb-2 block text-sm font-medium">

              Purpose

            </label>

            <select
              value={purpose}
              onChange={(e) => setPurpose(e.target.value)}
              className="w-full rounded-xl border p-3"
            >

              <option>Follow Up</option>
              <option>Meeting Reminder</option>
              <option>Thank You</option>
              <option>Proposal</option>

            </select>

          </div>

        </div>

        <div className="mt-8 flex justify-end gap-3">

          <button
            onClick={handleClose}
            className="rounded-xl border px-5 py-2"
          >

            Close

          </button>

          <button
            disabled={loading}
            onClick={() =>
              generate(customerId, {
                tone,
                purpose,
              })
            }
            className="rounded-xl bg-blue-600 px-5 py-2 font-medium text-white disabled:cursor-not-allowed disabled:opacity-50"
          >

            {loading
              ? "Generating..."
              : "Generate"}

          </button>

        </div>

        {error && (

          <div className="mt-6 rounded-xl border border-red-200 bg-red-50 p-4 text-red-600">

            {error}

          </div>

        )}

        {email && (

          <div className="mt-6">

            <h3 className="mb-3 text-lg font-semibold">

              Generated Email

            </h3>

            <div className="rounded-xl border bg-slate-50 p-5">

              <pre className="whitespace-pre-wrap font-sans">

                {email}

              </pre>

            </div>

            <div className="mt-5 flex justify-end gap-3">

              <button
                onClick={() =>
                  navigator.clipboard.writeText(email)
                }
                className="rounded-xl border px-4 py-2 transition hover:bg-slate-100"
              >

                Copy

              </button>

              <button
                disabled={loading}
                onClick={() =>
                  generate(customerId, {
                    tone,
                    purpose,
                  })
                }
                className="rounded-xl bg-blue-600 px-4 py-2 text-white transition hover:bg-blue-700 disabled:cursor-not-allowed disabled:opacity-50"
              >

                {loading
                  ? "Generating..."
                  : "Generate Again"}

              </button>

            </div>

          </div>

        )}

      </div>

    </div>

  );

}

export default CustomerFollowUpEmailModal;