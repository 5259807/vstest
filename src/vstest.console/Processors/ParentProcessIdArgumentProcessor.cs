// Copyright (c) Microsoft Corporation. All rights reserved.

namespace Microsoft.VisualStudio.TestPlatform.CommandLine.Processors
{
    using Microsoft.VisualStudio.TestPlatform.CommandLine;
    using System;
    using System.Diagnostics.Contracts;

    /// <summary>
    /// Argument Processor for the "/ParentProcessId" command line argument.
    /// </summary>
    internal class ParentProcessIdArgumentProcessor : IArgumentProcessor
    {
        #region Constants

        /// <summary>
        /// The name of the command line argument that the ParentProcessIdArgumentExecutor handles.
        /// </summary>
        public const string CommandName = "/ParentProcessId";

        #endregion

        private Lazy<IArgumentProcessorCapabilities> metadata;

        private Lazy<IArgumentExecutor> executor;

        /// <summary>
        /// Gets the metadata.
        /// </summary>
        public Lazy<IArgumentProcessorCapabilities> Metadata
        {
            get
            {
                if (this.metadata == null)
                {
                    this.metadata = new Lazy<IArgumentProcessorCapabilities>(() => new ParentProcessIdArgumentProcessorCapabilities());
                }

                return this.metadata;
            }
        }

        /// <summary>
        /// Gets or sets the executor.
        /// </summary>
        public Lazy<IArgumentExecutor> Executor
        {
            get
            {
                if (this.executor == null)
                {
                    this.executor = new Lazy<IArgumentExecutor>(() =>
                    new ParentProcessIdArgumentExecutor(CommandLineOptions.Instance));
                }

                return this.executor;
            }

            set
            {
                this.executor = value;
            }
        }
    }

    internal class ParentProcessIdArgumentProcessorCapabilities : BaseArgumentProcessorCapabilities
    {
        public override string CommandName => ParentProcessIdArgumentProcessor.CommandName;

        public override bool AllowMultiple => false;

        public override bool IsAction => false;

        public override ArgumentProcessorPriority Priority => ArgumentProcessorPriority.ParentProcessId;

        public override string HelpContentResourceName => Resources.ParentProcessIdArgumentHelp;

        public override HelpContentPriority HelpPriority => HelpContentPriority.ParentProcessIdArgumentProcessorHelpPriority;
    }

    /// <summary>
    /// Argument Executor for the "/ParentProcessId" command line argument.
    /// </summary>
    internal class ParentProcessIdArgumentExecutor : IArgumentExecutor
    {
        #region Fields

        /// <summary>
        /// Used for getting sources.
        /// </summary>
        private CommandLineOptions commandLineOptions;

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="options">
        /// The options.
        /// </param>
        public ParentProcessIdArgumentExecutor(CommandLineOptions options)
        {
            Contract.Requires(options != null);
            this.commandLineOptions = options;
        }

        #endregion

        /// <summary>
        /// Initializes with the argument that was provided with the command.
        /// </summary>
        /// <param name="argument">Argument that was provided with the command.</param>
        public void Initialize(string argument)
        {
            int parentProcessId;
            if (string.IsNullOrWhiteSpace(argument) || !int.TryParse(argument, out parentProcessId))
            {
                throw new CommandLineException(Resources.InvalidParentProcessIdArgument);
            }

            this.commandLineOptions.ParentProcessId = parentProcessId;
        }

        /// <summary>
        /// ParentProcessId is already set, return success.
        /// </summary>
        /// <returns> The <see cref="ArgumentProcessorResult"/> Success </returns>
        public ArgumentProcessorResult Execute()
        {
            // Nothing to do here, the work was done in initialization.
            return ArgumentProcessorResult.Success;
        }
    }
}
