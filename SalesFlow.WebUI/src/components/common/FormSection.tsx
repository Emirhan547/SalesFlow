import Card from "@/components/ui/Card";

type Props = {
  title: string;
  children: React.ReactNode;
};

function FormSection({
  title,
  children,
}: Props) {
  return (
    <Card title={title}>
      {children}
    </Card>
  );
}

export default FormSection;