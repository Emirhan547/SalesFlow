import ReactMarkdown from "react-markdown";

type Props = {
  insights: string;
};

function AIInsightsPanel({
  insights,
}: Props) {

  return (

    <div
      className="
        prose
        prose-slate
        max-w-none

        prose-headings:mt-8
        prose-headings:mb-4
        prose-headings:border-b
        prose-headings:border-slate-200
        prose-headings:pb-2
        prose-headings:text-slate-900

        prose-p:text-slate-700
        prose-p:leading-7

        prose-ul:space-y-2

        prose-li:marker:text-blue-500

        prose-strong:text-slate-900
        prose-strong:font-semibold
      "
    >

      <ReactMarkdown

        components={{

          h2: ({ children }) => (

            <div className="mt-8">

              <div className="mb-3 flex items-center gap-2">

                <div className="h-2 w-2 rounded-full bg-blue-500" />

                <h2 className="text-lg font-semibold">

                  {children}

                </h2>

              </div>

            </div>

          ),

          h3: ({ children }) => (

            <h3 className="mt-6 text-base font-semibold">

              {children}

            </h3>

          ),

          blockquote: ({ children }) => (

            <div className="rounded-xl border-l-4 border-blue-500 bg-blue-50 p-4">

              {children}

            </div>

          ),

          ul: ({ children }) => (

            <ul className="space-y-2 pl-5">

              {children}

            </ul>

          ),

          li: ({ children }) => (

            <li className="leading-7">

              {children}

            </li>

          ),

        }}

      >

        {insights}

      </ReactMarkdown>

    </div>

  );

}

export default AIInsightsPanel;