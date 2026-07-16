import { useNavigate } from "react-router-dom";

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
    <div className="flex justify-end gap-4">

      <Button
        type="button"
        variant="outline"
        onClick={() =>
          navigate("/deals")
        }
      >
        Cancel
      </Button>

      <Button
        type="submit"
        disabled={loading}
      >
        {loading
          ? "Saving..."
          : submitText}
      </Button>

    </div>
  );
}

export default FormButtons;