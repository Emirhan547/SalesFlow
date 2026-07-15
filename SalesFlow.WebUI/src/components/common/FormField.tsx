import { Label } from "@/components/ui/label";

type Props = {
  label: string;
  error?: string;
  children: React.ReactNode;
};

function FormField({
  label,
  error,
  children,
}: Props) {
  return (
    <div>

      <Label>
        {label}
      </Label>

      <div className="mt-2">

        {children}

      </div>

      {error && (

        <p className="mt-2 text-sm text-red-500">
          {error}
        </p>

      )}

    </div>
  );
}

export default FormField;