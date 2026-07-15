import { useNavigate } from "react-router-dom";

import { ArrowLeft, Save } from "lucide-react";

import { Button } from "@/components/ui/button";

type Props = {
  loading: boolean;
  submitText: string;
};

function FormButtons({
  loading,
  submitText,
}: Props) {

  const navigate =
    useNavigate();

  return (
    <div className="flex justify-end gap-4 border-t border-slate-200 pt-6">

      <Button
        type="button"
        variant="outline"
        onClick={() =>
          navigate("/customers")
        }
      >
        <ArrowLeft
          size={16}
          className="mr-2"
        />

        Cancel

      </Button>

      <Button
        type="submit"
        disabled={loading}
      >
        <Save
          size={16}
          className="mr-2"
        />

        {loading
          ? "Saving..."
          : submitText}

      </Button>

    </div>
  );
}

export default FormButtons;